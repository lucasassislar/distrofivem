let objFullScreenMenu;

let arrInventoryItems;
let nLastHighlight = -1;

function clearInventory() {
    for (let i = 0; i < 9; i++) {
        arrInventoryItems[i].innerHTML = '';
    }
}

function receiveInventory(nSlot, itemName) {
    const objImg = document.createElement('img');
    objImg.src = `nui://distroclient/ui/resources/items/${itemName}.jpg`;
    objImg.classList.add('inventoryItemImage');

    arrInventoryItems[nSlot].appendChild(objImg);
}

function inventoryUnhighlight() {
    for (let i = 0; i < arrInventoryItems.length; i++) {
        arrInventoryItems[i].classList.remove('inventoryItemHighlight');
    }
}

function inventoryHighlight(nSlot) {
    inventoryUnhighlight();

    nLastHighlight = nSlot;

    arrInventoryItems[nSlot].classList.add('inventoryItemHighlight');
}

document.addEventListener('DOMContentLoaded', function(event) {
    objFullScreenMenu = document.getElementById('fullScreenMenu');

    arrInventoryItems = new Array(0);
    for (let i = 0; i < 9; i++) {
        arrInventoryItems.push(document.getElementById(`inventory${i + 1}`));
    }

    window.addEventListener('message', function(event) {
        if (event.data.type === 'showFullscreenUI') {
            objFullScreenMenu.classList.remove('invisible');
        } else if (event.data.type === 'clearInventory') {
            clearInventory();
        } else if (event.data.type === 'receiveInventory') {
            receiveInventory(event.data.slot, event.data.item);
        } else if (event.data.type === 'inventoryHighlight') {
            inventoryHighlight(event.data.slot);
        } else if (event.data.type === 'inventoryUnhighlight') {
            inventoryUnhighlight();
        }
    });

    document.onkeyup = function(data) {
        // Escape
        if (data.which == 27) {
            objFullScreenMenu.classList.add('invisible');

            $.post('http://distroclient/CloseFullscreenMenu', JSON.stringify({}));
        }
    }
});