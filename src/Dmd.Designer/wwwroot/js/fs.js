var fs = require('fs');

function getDirectoryChildrenInternal(path) {
    var nodes = [];

    fs.readdirSync(path).forEach(info => {
        if (info === '.git' || info === '.vs' || info === '.vscode' || info === 'bin' || info === 'obj') {
            return;
        }
        var stat = fs.statSync(path + "\\" + info);
        if (stat.isDirectory()) {

            var node = {
                path: path + "\\" + info,
                name: info,
                children: []
            }
            node.children = getDirectoryChildrenInternal(path + "\\" + info);
            nodes.push(node);
        }
    });
    return nodes;
}

export function getDirectoryChildren(path) {
    return JSON.stringify(getDirectoryChildrenInternal(path));
}

//var root = loadDirectories('C:/Users/Chi/source/repos/Dmd');
//var directories = "C:/Users/Chi/Desktop/directories.json";// you need to save the filepath when you open the file to update without use the filechooser dialog againg
//var content = JSON.stringify(root);

//fs.writeFile(directories, content, (err) => {
//    if (err) {
//        alert("An error ocurred updating the file" + err.message);
//        console.log(err);
//        return;
//    }

//    alert("The file has been succesfully saved");
//});