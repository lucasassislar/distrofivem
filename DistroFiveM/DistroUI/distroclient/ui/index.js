let objFullScreenMenu;

document.addEventListener('DOMContentLoaded', function (event) {
    objFullScreenMenu = document.getElementById('fullScreenMenu');

    window.addEventListener('message', function (event) {
        if (event.data.type === 'showFullscreenUI') {
            objFullScreenMenu.classList.remove('invisible');
        }
    });

    document.onkeyup = function (data) {
        // Escape
        if (data.which == 27) {
            objFullScreenMenu.classList.add('invisible');
            
            $.post('http://distroclient/CloseFullscreenMenu', JSON.stringify({}));
        }
    }
});