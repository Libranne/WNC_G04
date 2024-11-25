document.getElementById('showMoreBtn').addEventListener('click', function () {
    var menu = document.getElementById('moreOptions');
    // Toggle the visibility of the menu
    if (menu.style.display === 'none' || menu.style.display === '') {
        menu.style.display = 'block';
    } else {
        menu.style.display = 'none';
    }
});
