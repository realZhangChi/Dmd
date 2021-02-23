// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.

let canvas;

export function init(id) {
    canvas = new fabric.Canvas(id);

    let isDragging = false;

    for (let x = -50000; x <= 50000; x += 20) {
        let strokeWidth = 1;
        if (x % 100 === 0) {
            strokeWidth = 2;
        }
        let line = new fabric.Line([x, -50000, x, 50000], {
            stroke: 'rgba(211,211,211,1)',
            strokeWidth: strokeWidth,
            selectable: false
        });
        canvas.add(line);
    }
    for (let y = -50000; y <= 50000; y += 20) {
        let strokeWidth = 1;
        if (y % 100 === 0) {
            strokeWidth = 2;
        }
        let line = new fabric.Line([-50000, y, 50000, y], {
            stroke: 'rgba(211,211,211,1)',
            strokeWidth: strokeWidth,
            selectable: false
        });
        canvas.add(line);
    }
    canvas.add(new fabric.Rect({ width: 50, height: 50, fill: 'blue', angle: 0 }));

    canvas.on('mouse:down', function(opt) {
        var evt = opt.e;
        if (evt.altKey === true) {
            this.isDragging = true;
            this.selection = false;
            this.lastPosX = evt.clientX;
            this.lastPosY = evt.clientY;
        }
    });

    canvas.on('mouse:move', function(opt) {
        if (this.isDragging) {
            var e = opt.e;
            var vpt = this.viewportTransform;
            vpt[4] += e.clientX - this.lastPosX;
            vpt[5] += e.clientY - this.lastPosY;
            this.requestRenderAll();
            this.lastPosX = e.clientX;
            this.lastPosY = e.clientY;
        }
    });

    canvas.on('mouse:up', function(opt) {
        // on mouse up we want to recalculate new interaction
        // for all objects, so we call setViewportTransform
        this.setViewportTransform(this.viewportTransform);
        this.isDragging = false;
        this.selection = true;
    });

    canvas.on('mouse:wheel',
        function (opt) {
            var delta = opt.e.deltaY;
            var zoom = canvas.getZoom();
            zoom *= 0.999 ** delta;
            if (zoom > 5) zoom = 5;
            if (zoom < 0.5) zoom = 0.5;
            canvas.zoomToPoint({ x: opt.e.offsetX, y: opt.e.offsetY }, zoom);
            opt.e.preventDefault();
            opt.e.stopPropagation();
        });
}
