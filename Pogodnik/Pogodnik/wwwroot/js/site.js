// Основные функции для Pogodnik

// Функция для форматирования даты
function formatDate(date, format = 'short') {
    const d = new Date(date);

    if (format === 'short') {
        return d.toLocaleDateString('ru-RU', {
            day: '2-digit',
            month: '2-digit',
            year: 'numeric'
        });
    } else if (format === 'long') {
        return d.toLocaleDateString('ru-RU', {
            weekday: 'long',
            day: '2-digit',
            month: 'long',
            year: 'numeric'
        });
    } else if (format === 'time') {
        return d.toLocaleTimeString('ru-RU', {
            hour: '2-digit',
            minute: '2-digit'
        });
    }

    return d.toLocaleDateString('ru-RU');
}

// Функция для форматирования температуры
function formatTemperature(temp, unit = 'C') {
    const rounded = Math.round(temp);
    return `${rounded}°${unit}`;
}

// Функция для получения иконки погоды по коду
function getWeatherIcon(code, size = 2) {
    // code: '01d', '02n', '10d', etc.
    return `https://openweathermap.org/img/wn/${code}@${size}x.png`;
}

// Функция для получения описания направления ветра по градусам
function getWindDirection(degrees) {
    if (degrees >= 337.5 || degrees < 22.5) return 'С';
    if (degrees >= 22.5 && degrees < 67.5) return 'СВ';
    if (degrees >= 67.5 && degrees < 112.5) return 'В';
    if (degrees >= 112.5 && degrees < 157.5) return 'ЮВ';
    if (degrees >= 157.5 && degrees < 202.5) return 'Ю';
    if (degrees >= 202.5 && degrees < 247.5) return 'ЮЗ';
    if (degrees >= 247.5 && degrees < 292.5) return 'З';
    return 'СЗ';
}

// Функция для расчета индекса комфорта
function calculateComfortIndex(temp, humidity) {
    // Простая формула для определения комфортности погоды
    if (temp >= 25 && humidity >= 70) return 'Жарко и душно';
    if (temp >= 20 && temp < 25 && humidity >= 60 && humidity < 70) return 'Тепло и влажно';
    if (temp >= 18 && temp < 22 && humidity >= 40 && humidity < 60) return 'Идеально';
    if (temp >= 15 && temp < 18 && humidity >= 30 && humidity < 50) return 'Прохладно';
    if (temp < 10) return 'Холодно';
    return 'Нормально';
}

// Функция для конвертации единиц измерения
function convertUnits(value, fromUnit, toUnit) {
    if (fromUnit === toUnit) return value;

    // Конвертация температуры
    if (fromUnit === 'C' && toUnit === 'F') {
        return (value * 9 / 5) + 32;
    } else if (fromUnit === 'F' && toUnit === 'C') {
        return (value - 32) * 5 / 9;
    }

    // Конвертация скорости ветра
    if (fromUnit === 'm/s' && toUnit === 'km/h') {
        return value * 3.6;
    } else if (fromUnit === 'km/h' && toUnit === 'm/s') {
        return value / 3.6;
    }

    return value;
}

// Функция для проверки поддержки геолокации
function checkGeolocationSupport() {
    return 'geolocation' in navigator;
}

// Функция для получения местоположения пользователя
function getUserLocation() {
    return new Promise((resolve, reject) => {
        if (!checkGeolocationSupport()) {
            reject(new Error('Геолокация не поддерживается вашим браузером'));
            return;
        }

        navigator.geolocation.getCurrentPosition(
            position => {
                resolve({
                    latitude: position.coords.latitude,
                    longitude: position.coords.longitude,
                    accuracy: position.coords.accuracy
                });
            },
            error => {
                let message = 'Не удалось получить ваше местоположение';

                switch (error.code) {
                    case error.PERMISSION_DENIED:
                        message = 'Доступ к геолокации запрещен';
                        break;
                    case error.POSITION_UNAVAILABLE:
                        message = 'Информация о местоположении недоступна';
                        break;
                    case error.TIMEOUT:
                        message = 'Время ожидания истекло';
                        break;
                }

                reject(new Error(message));
            },
            {
                enableHighAccuracy: true,
                timeout: 10000,
                maximumAge: 60000
            }
        );
    });
}

// Функция для определения города по координатам (заглушка)
function getCityByCoords(lat, lon) {
    // В реальном приложении здесь должен быть запрос к API геокодирования
    console.log(`Координаты: ${lat}, ${lon}`);
    return Promise.resolve('Воронеж'); // Заглушка
}

// Функция для сохранения настроек пользователя
function saveUserSettings(settings) {
    try {
        localStorage.setItem('weatherSettings', JSON.stringify(settings));
        return true;
    } catch (error) {
        console.error('Ошибка сохранения настроек:', error);
        return false;
    }
}

// Функция для загрузки настроек пользователя
function loadUserSettings() {
    try {
        const settings = localStorage.getItem('weatherSettings');
        return settings ? JSON.parse(settings) : {
            temperatureUnit: 'C',
            windSpeedUnit: 'm/s',
            pressureUnit: 'hPa',
            language: 'ru',
            defaultCity: 'Воронеж',
            showNotifications: true,
            theme: 'light'
        };
    } catch (error) {
        console.error('Ошибка загрузки настроек:', error);
        return {
            temperatureUnit: 'C',
            windSpeedUnit: 'm/s',
            pressureUnit: 'hPa',
            language: 'ru',
            defaultCity: 'Воронеж',
            showNotifications: true,
            theme: 'light'
        };
    }
}

// Функция для показа уведомления
function showNotification(title, message, type = 'info') {
    if (!('Notification' in window)) {
        console.log('Браузер не поддерживает уведомления');
        return;
    }

    if (Notification.permission === 'granted') {
        const notification = new Notification(title, {
            body: message,
            icon: '/favicon.ico'
        });

        notification.onclick = function () {
            window.focus();
            notification.close();
        };

        setTimeout(notification.close.bind(notification), 5000);
    } else if (Notification.permission !== 'denied') {
        Notification.requestPermission().then(permission => {
            if (permission === 'granted') {
                showNotification(title, message, type);
            }
        });
    }
}

// Функция для запроса разрешения на уведомления
function requestNotificationPermission() {
    if (!('Notification' in window)) {
        return Promise.reject(new Error('Браузер не поддерживает уведомления'));
    }

    return Notification.requestPermission();
}

// Функция для проверки, поддерживается ли LocalStorage
function isLocalStorageSupported() {
    try {
        const test = '__localStorage_test__';
        localStorage.setItem(test, test);
        localStorage.removeItem(test);
        return true;
    } catch (error) {
        console.error('LocalStorage не поддерживается:', error);
        return false;
    }
}

// Функция для копирования текста в буфер обмена
function copyToClipboard(text) {
    return navigator.clipboard.writeText(text)
        .then(() => {
            console.log('Текст скопирован в буфер обмена');
            return true;
        })
        .catch(err => {
            console.error('Ошибка копирования в буфер обмена:', err);

            // Fallback для старых браузеров
            const textArea = document.createElement('textarea');
            textArea.value = text;
            document.body.appendChild(textArea);
            textArea.select();

            try {
                document.execCommand('copy');
                console.log('Текст скопирован (fallback)');
                return true;
            } catch (err) {
                console.error('Fallback также не сработал:', err);
                return false;
            } finally {
                document.body.removeChild(textArea);
            }
        });
}

// Функция для генерации уникального ID
function generateId() {
    return Date.now().toString(36) + Math.random().toString(36).substr(2);
}

// Функция для форматирования чисел с разделителями
function formatNumber(num, decimals = 0) {
    return num.toLocaleString('ru-RU', {
        minimumFractionDigits: decimals,
        maximumFractionDigits: decimals
    });
}

// Функция для расчета индекса УФ-излучения (упрощенная)
function calculateUVIndex(temp, cloudiness) {
    // Упрощенный расчет УФ-индекса на основе температуры и облачности
    let baseIndex = (temp - 10) / 5; // Базовый индекс от температуры
    baseIndex = Math.max(0, Math.min(baseIndex, 5)); // Ограничиваем 0-5

    // Корректировка на облачность
    const cloudFactor = (100 - cloudiness) / 100;

    return Math.round(baseIndex * cloudFactor * 2); // Масштабируем до 0-10
}

// Инициализация при загрузке страницы
document.addEventListener('DOMContentLoaded', function () {
    console.log('Pogodnik загружен');

    // Загружаем настройки пользователя
    const settings = loadUserSettings();
    console.log('Настройки пользователя:', settings);

    // Применяем тему
    if (settings.theme === 'dark') {
        document.body.classList.add('dark-theme');
    }

    // Проверяем поддержку LocalStorage
    if (!isLocalStorageSupported()) {
        console.warn('LocalStorage не поддерживается, некоторые функции могут быть недоступны');
    }

    // Обновляем время каждую секунду
    function updateClock() {
        const now = new Date();
        const clockElements = document.querySelectorAll('.current-time');

        clockElements.forEach(el => {
            el.textContent = now.toLocaleTimeString('ru-RU', {
                hour: '2-digit',
                minute: '2-digit',
                second: '2-digit'
            });
        });
    }

    setInterval(updateClock, 1000);
    updateClock();

    // Инициализация tooltips Bootstrap
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Инициализация popovers Bootstrap
    const popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl);
    });

    // Обработка форм с предотвращением двойной отправки
    const forms = document.querySelectorAll('form');
    forms.forEach(form => {
        form.addEventListener('submit', function () {
            const submitButton = this.querySelector('button[type="submit"]');
            if (submitButton) {
                submitButton.disabled = true;
                submitButton.innerHTML = '<span class="spinner-border spinner-border-sm" role="status"></span> Загрузка...';

                // Восстанавливаем кнопку через 5 секунд на случай ошибки
                setTimeout(() => {
                    submitButton.disabled = false;
                    submitButton.innerHTML = submitButton.getAttribute('data-original-text') || 'Отправить';
                }, 5000);
            }
        });
    });

    // Сохраняем оригинальный текст кнопок
    document.querySelectorAll('button[type="submit"]').forEach(button => {
        button.setAttribute('data-original-text', button.textContent);
    });

    // Обработка ошибок сети
    window.addEventListener('online', function () {
        showNotification('Pogodnik', 'Соединение восстановлено', 'success');
    });

    window.addEventListener('offline', function () {
        showNotification('Pogodnik', 'Нет соединения с интернетом', 'warning');
    });

    // Регистрация Service Worker для PWA
    if ('serviceWorker' in navigator) {
        navigator.serviceWorker.register('/service-worker.js')
            .then(registration => {
                console.log('Service Worker зарегистрирован:', registration);
            })
            .catch(error => {
                console.log('Ошибка регистрации Service Worker:', error);
            });
    }
});

// Экспорт функций для использования в других модулях
if (typeof module !== 'undefined' && module.exports) {
    module.exports = {
        formatDate,
        formatTemperature,
        getWeatherIcon,
        getWindDirection,
        calculateComfortIndex,
        convertUnits,
        checkGeolocationSupport,
        getUserLocation,
        getCityByCoords,
        saveUserSettings,
        loadUserSettings,
        showNotification,
        requestNotificationPermission,
        isLocalStorageSupported,
        copyToClipboard,
        generateId,
        formatNumber,
        calculateUVIndex
    };
}