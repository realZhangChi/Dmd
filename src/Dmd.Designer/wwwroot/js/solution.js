var fs = require('fs');

function getSolutionTreeInternal(path) {
    var nodes = [];

    fs.readdirSync(path).forEach(info => {
        if (info.startsWith('.') || info === 'bin' || info === 'obj') {
            return;
        }
        var stat = fs.statSync(path + "\\" + info);
        if (stat.isDirectory()) {

            var directory = {
                fullPath: path + "\\" + info,
                fileType: 1,
                children: []
            }
            directory.children = getSolutionTreeInternal(path + "\\" + info);
            nodes.push(directory);
        } else {
            if (!info.endsWith('.csropj')) {
                return;
            }
            var file = {
                fullPath: path + "\\" + info,
                fileType: 0,
                children: []
            }
            nodes.push(file);
        }
    });
    return nodes;
}

export function getSolutionTree(path) {
    return JSON.stringify(getSolutionTreeInternal(path));
}
