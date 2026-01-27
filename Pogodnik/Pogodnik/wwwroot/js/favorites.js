// Обработчик кнопки избранного в навигации
document.addEventListener('DOMContentLoaded', function () {
    const showFavoritesBtn = document.getElementById('showFavoritesBtn');
    if (showFavoritesBtn) {
        showFavoritesBtn.addEventListener('click', function (e) {
            e.preventDefault();
            window.favoritesManager.showFavoritesModal();
        });
    }

    // Обновляем счетчик избранного
    function updateFavoritesCount() {
        const count = window.favoritesManager.getAllFavorites().length;
        const countElement = document.getElementById('favoritesCount');
        if (countElement) {
            countElement.textContent = count;
            countElement.style.display = count > 0 ? 'inline-block' : 'none';
        }
    }

    // Инициализация счетчика
    updateFavoritesCount();

    // Обновляем счетчик при изменениях
    const originalAddFavorite = window.favoritesManager.addFavorite;
    window.favoritesManager.addFavorite = function (...args) {
        const result = originalAddFavorite.apply(this, args);
        updateFavoritesCount();
        return result;
    };

    const originalRemoveFavorite = window.favoritesManager.removeFavorite;
    window.favoritesManager.removeFavorite = function (...args) {
        const result = originalRemoveFavorite.apply(this, args);
        updateFavoritesCount();
        return result;
    };

    const originalClearFavorites = window.favoritesManager.clearFavorites;
    window.favoritesManager.clearFavorites = function (...args) {
        const result = originalClearFavorites.apply(this, args);
        updateFavoritesCount();
        return result;
    };
});