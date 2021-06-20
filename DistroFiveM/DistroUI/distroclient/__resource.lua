client_script 'DistroClient.net.dll'

-- specify the root page, relative to the resource
ui_page 'ui/index.html'

-- every client-side file still needs to be added to the resource packfile!
files {
    'ui/index.html',
    'ui/index.css',
    'ui/index.js'
}