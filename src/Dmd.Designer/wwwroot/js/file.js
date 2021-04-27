var fs = require('fs');

export function save(directory, name, content) {
    return new window.Promise((resolve, reject) => {
        fs.mkdir(directory, { recursive: true }, (err) => {
            if (err) {
                console.debug(err);
                reject(err);
                return;
            }

            fs.writeFile(directory + '\\' + name, content, (err) => {
                if (err) {
                    console.log(err);
                    reject(err);
                    return;
                }

                resolve();
            });
        });
    });
}

export function readFile(fullPath) {
    return new window.Promise((resolve, reject) => {
        fs.readFile(fullPath,
            (err, data) => {
                if (err) {
                    console.debug(err);
                    if (err.code === 'ENOENT') {
                        resolve();
                    }
                    reject(err);
                    return;
                }
                resolve(data.toString());
            });
    });
}

export function access(fullPath) {
    fs.access(fullPath, fs.constants.F_OK | fs.constants.W_OK, (err) => {
        if (err) {
            console.debug(
                `${fullPath} ${err.code === 'ENOENT' ? 'does not exist' : 'is read-only'}`);
            return false;
        } else {
            return true;
        }
    });
}
