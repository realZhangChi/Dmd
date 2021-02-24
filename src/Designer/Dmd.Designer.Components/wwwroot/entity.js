
fabric.CustomGroup = fabric.util.createClass(fabric.Group, {

    type: 'customGroup',

    initialize: function (parameters) {
        var options = {};

        this.set('entityName', parameters.entityName || '');
        this.set('properties', parameters.properties || []);
        this.set('methods', parameters.methods || []);

        var nameHeight = 20;
        var propertyAndMethodHeight = 24;
        options.shadow = { color: "rgba(0, 0, 0, 0.3)", blur: 20, offsetX: 5, offsetY: 5 };
        options.width = 150;
        options.height = nameHeight + (this.properties.length + this.methods.length) * propertyAndMethodHeight + 12;

        var objects = [];

        var background = new fabric.Rect({
            width: options.width,
            height: options.height,
            originX: 'center',
            originY: 'center',
            fill: 'rgba(248,249,250,1)'
        });
        objects.push(background);

        var top = -options.height / 2;
        var nameTextBox = new fabric.Textbox(this.entityName,
            {
                width: options.width,
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
        top += 6;

        for (var i = 0; i < this.properties.length; i++, top += propertyAndMethodHeight) {
            var property = new fabric.Textbox(this.properties[i],
                {
                    width: options.width,
                    height: propertyAndMethodHeight,
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
        
        var line = new fabric.Line([-options.width / 2, top, options.width / 2, top], {
            stroke: '#007bff',
            selectable: false
        });
        objects.push(line);
        top += 6;

        for (var j = 0; j < this.methods.length; j++, top += propertyAndMethodHeight) {
            var method = new fabric.Textbox(this.methods[j],
                {
                    width: options.width,
                    height: propertyAndMethodHeight,
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
        this.callSuper('initialize', objects, options, true);
    },

    toObject: function () {
        return fabric.util.object.extend(this.callSuper('toObject'), {
        });
    },

    _render: function (ctx) {
        this.callSuper('_render', ctx);
    }
});