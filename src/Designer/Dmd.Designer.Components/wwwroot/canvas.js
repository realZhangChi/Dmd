// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.

let canvas;

export function init(id) {
    canvas = new fabric.Canvas(id);

    let width = canvas.getWidth();
    let height = canvas.getHeight();

    for (let x = 0; x <= width; x += 20) {
        let line = new fabric.Line([x, 0, x, height], {
            stroke: 'red',
            selectable: false
        });
        canvas.add(line);
    }
    for (let y = 0; y <= height; y += 20) {
        let line = new fabric.Line([0, y, width, y], {
            stroke: 'red',
            selectable: false
        });
        canvas.add(line);
    }

    canvas.on('mouse:wheel', function (opt) {
        var delta = opt.e.deltaY;
        var zoom = canvas.getZoom();
        zoom *= 0.999 ** delta;
        if (zoom > 20) zoom = 20;
        if (zoom < 0.01) zoom = 0.01;
        canvas.setZoom(zoom);
        opt.e.preventDefault();
        opt.e.stopPropagation();
    })
}
