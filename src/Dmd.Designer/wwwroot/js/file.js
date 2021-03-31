var fs = require('fs');

export function save(directory, name, content) {
    return new window.Promise((resolve, reject) => {
        fs.writeFile(directory + '/' + name, content, (err) => {
            if (err) {
                console.log(err);
                reject(err);
                return;
            }

            resolve();
        });
    });
}

export function readJsonFile(fullPath) {
    return new window.Promise((resolve, reject) => {
        fs.readFile(fullPath,
            (err, data) => {
                if (err) {
                    reject(err);
                    return;
                }
                resolve(data.toString());
            });
    });
}
