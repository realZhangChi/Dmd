// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.

var canvas;

export function init(id) {
    canvas = new fabric.Canvas(id);
    setDimensions();
    new ResizeObserver(setDimensions).observe(document.getElementsByClassName('canvas-container')[0].parentElement);
    //document.getElementsByClassName('canvas-container')[0].parentElement.onresize = function () { setDimensions(); };

    var isDragging = false;

    for (var x = -50000; x <= 50000; x += 20) {
        var strokeWidth = 1;
        if (x % 100 === 0) {
            strokeWidth = 2;
        }
        var line = new fabric.Line([x, -50000, x, 50000], {
            stroke: 'rgba(240,248,255,1)',
            strokeWidth: strokeWidth,
            selectable: false
        });
        canvas.add(line);
    }
    for (var y = -50000; y <= 50000; y += 20) {
        var strokeWidth = 1;
        if (y % 100 === 0) {
            strokeWidth = 2;
        }
        var line = new fabric.Line([-50000, y, 50000, y], {
            stroke: 'rgba(240,248,255,1)',
            strokeWidth: strokeWidth,
            selectable: false
        });
        canvas.add(line);
    }

    canvas.on('mouse:down', function (opt) {
        var evt = opt.e;
        if (evt.altKey === true) {
            this.isDragging = true;
            this.selection = false;
            this.lastPosX = evt.clientX;
            this.lastPosY = evt.clientY;
        }
    });

    canvas.on('mouse:move', function (opt) {
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

    canvas.on('mouse:up', function (opt) {
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
};

function setDimensions() {
    var parent = document.getElementsByClassName('canvas-container')[0].parentElement;
    canvas.setDimensions({
        width: parent.offsetWidth,
        height: parent.offsetHeight
    });
}

export function addClass(name, properties, methods, position) {

    var classComponent = new fabric.ClassComponent({
        className: name,
        properties: properties,
        methods: methods,
        left: position[0] ?? 100,
        top: position[1] ?? 100,
    });
    classComponent.setControlsVisibility({
        mt: false,
        mb: false,
        ml: false,
        mr: false,
        bl: false,
        br: false,
        tl: false,
        tr: false,
        mtr: false,
    });
    canvas.add(classComponent);
}


fabric.ClassComponent = fabric.util.createClass(fabric.Group, {

    type: 'ClassComponent',

    initialize: function (options) {
        this.set('className', options.className || '');
        this.set('properties', options.properties || []);
        this.set('methods', options.methods || []);

        var nameHeight = 20;
        var itemHeight = 24;
        var partialSpacing = 6;
        var partQuantity = 3;
        var innerOptions = {};
        innerOptions.shadow = { color: "rgba(0, 0, 0, 0.3)", blur: 10, offsetX: 3, offsetY: 3 };
        innerOptions.width = 180;
        innerOptions.height = nameHeight +
            (this.properties.length + this.methods.length) * itemHeight +
            (partQuantity - 1) * partialSpacing;
        innerOptions.left = options.left - innerOptions.width / 2;
        innerOptions.top = options.top - innerOptions.height / 2;

        var objects = [];

        // background
        var background = new fabric.Rect({
            width: innerOptions.width,
            height: innerOptions.height,
            originX: 'center',
            originY: 'center',
            fill: 'rgba(248,249,250,1)'
        });
        objects.push(background);

        // name part
        var top = -innerOptions.height / 2;
        var nameTextBox = new fabric.Textbox(this.className,
            {
                width: innerOptions.width,
                height: 100,
                originX: 'center',
                originY: 'top',
                top: top,
                textAlign: 'left',
                fontSize: 18,
                fill: '#fff',
                fontFamily: "'Helvetica Neue', Helvetica, Arial, sans-serif",
                backgroundColor: 'rgba(0,123,255,1)'
            });
        top += nameHeight;
        objects.push(nameTextBox);
        top += partialSpacing;

        // property part
        for (var i = 0; i < this.properties.length; i++, top += itemHeight) {
            var property = new fabric.Textbox(this.properties[i],
                {
                    width: innerOptions.width,
                    height: itemHeight,
                    originX: 'top',
                    originY: 'top',
                    top: top,
                    textAlign: 'left',
                    fontSize: 14,
                    fill: '#343a40',
                    fontFamily: "'Helvetica Neue', Helvetica, Arial, sans-serif"
                });
            objects.push(property);
        }
        if (this.methods.length > 0) {
            var line = new fabric.Line([-innerOptions.width / 2, top, innerOptions.width / 2, top], {
                stroke: '#007bff',
                selectable: false
            });
            objects.push(line);
            top += partialSpacing;
        }

        // method part
        for (var j = 0; j < this.methods.length; j++, top += itemHeight) {
            var method = new fabric.Textbox(this.methods[j],
                {
                    width: innerOptions.width,
                    height: itemHeight,
                    originX: 'top',
                    originY: 'top',
                    top: top,
                    textAlign: 'left',
                    fontSize: 14,
                    fill: '#343a40',
                    fontFamily: "'Helvetica Neue', Helvetica, Arial, sans-serif"
                });
            objects.push(method);
        }

        // initialize
        this.callSuper('initialize', objects, innerOptions, true);
    },

    toObject: function () {
        return fabric.util.object.extend(this.callSuper('toObject'), {
        });
    },

    _render: function (ctx) {
        this.callSuper('_render', ctx);
    }
});

