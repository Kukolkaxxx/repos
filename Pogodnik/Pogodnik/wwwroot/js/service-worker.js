// Service Worker для Pogodnik

const CACHE_NAME = 'pogodnik-v1.0';
const urlsToCache = [
    '/',
    '/css/site.css',
    '/css/weather-animations.css',
    '/js/site.js',
    '/js/favorites.js',
    '/favicon.ico',
    'https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css',
    'https://cdnjs.cloudflare.com/ajax/libs/animate.css/4.1.1/animate.min.css',
    'https://cdn.jsdelivr.net/npm/chart.js'
];

// Установка Service Worker
self.addEventListener('install', event => {
    event.waitUntil(
        caches.open(CACHE_NAME)
            .then(cache => {
                console.log('Кэш открыт');
                return cache.addAll(urlsToCache);
            })
            .then(() => self.skipWaiting())
    );
});

// Активация Service Worker
self.addEventListener('activate', event => {
    event.waitUntil(
        caches.keys().then(cacheNames => {
            return Promise.all(
                cacheNames.map(cacheName => {
                    if (cacheName !== CACHE_NAME) {
                        console.log('Удаляем старый кэш:', cacheName);
                        return caches.delete(cacheName);
                    }
                })
            );
        }).then(() => self.clients.claim())
    );
});

// Перехват запросов
self.addEventListener('fetch', event => {
    // Исключаем запросы к API
    if (event.request.url.includes('/api/') ||
        event.request.url.includes('openweathermap.org')) {
        return;
    }

    event.respondWith(
        caches.match(event.request)
            .then(response => {
                if (response) {
                    return response;
                }

                return fetch(event.request)
                    .then(response => {
                        // Проверяем, валидный ли ответ
                        if (!response || response.status !== 200 || response.type !== 'basic') {
                            return response;
                        }

                        // Клонируем ответ
                        const responseToCache = response.clone();

                        caches.open(CACHE_NAME)
                            .then(cache => {
                                cache.put(event.request, responseToCache);
                            });

                        return response;
                    })
                    .catch(() => {
                        // Оффлайн-страница
                        if (event.request.mode === 'navigate') {
                            return caches.match('/');
                        }

                        // Для других ресурсов
                        return new Response('Оффлайн режим', {
                            status: 503,
                            statusText: 'Service Unavailable',
                            headers: new Headers({
                                'Content-Type': 'text/plain'
                            })
                        });
                    });
            })
    );
});

// Фоновая синхронизация
self.addEventListener('sync', event => {
    if (event.tag === 'sync-weather') {
        event.waitUntil(syncWeatherData());
    }
});

// Фоновая задача для синхронизации погодных данных
function syncWeatherData() {
    console.log('Синхронизация погодных данных...');
    // Здесь можно добавить логику синхронизации
    return Promise.resolve();
}

// Push-уведомления
self.addEventListener('push', event => {
    let data = {};

    if (event.data) {
        data = event.data.json();
    }

    const options = {
        body: data.body || 'Новое уведомление от Pogodnik',
        icon: '/favicon.ico',
        badge: '/favicon.ico',
        vibrate: [100, 50, 100],
        data: {
            dateOfArrival: Date.now(),
            primaryKey: 'weather-notification'
        },
        actions: [
            {
                action: 'explore',
                title: 'Открыть',
                icon: '/favicon.ico'
            },
            {
                action: 'close',
                title: 'Закрыть',
                icon: '/favicon.ico'
            }
        ]
    };

    event.waitUntil(
        self.registration.showNotification(data.title || 'Pogodnik', options)
    );
});

self.addEventListener('notificationclick', event => {
    event.notification.close();

    if (event.action === 'close') {
        return;
    }

    event.waitUntil(
        clients.matchAll({
            type: 'window'
        }).then(clientList => {
            for (const client of clientList) {
                if (client.url === '/' && 'focus' in client) {
                    return client.focus();
                }
            }

            if (clients.openWindow) {
                return clients.openWindow('/');
            }
        })
    );
});