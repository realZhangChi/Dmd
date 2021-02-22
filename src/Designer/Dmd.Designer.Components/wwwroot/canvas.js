// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.

let canvas;

export function showPrompt(id) {


    canvas = new fabric.Canvas(id);

    var rect = new fabric.Rect({
        left: 100,
        top: 100,
        fill: 'red',
        width: 20,
        height: 20
    });

    canvas.add(rect);
}
