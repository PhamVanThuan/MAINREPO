
(function () {
    var tokenRegex = /\{([^\}]+)\}/g,
        objNotationRegex = /(?:(?:^|\.)(.+?)(?=\[|\.|$|\()|\[('|")(.+?)\2\])(\(\))?/g, // matches .xxxxx or ["xxxxx"] to run over object properties
        replacer = function (all, key, obj) {
            var res = obj;
            key.replace(objNotationRegex, function (all, name, quote, quotedName, isFunc) {
                name = name || quotedName;
                if (res) {
                    if (name in res) {
                        res = res[name];
                    }
                    typeof res == "function" && isFunc && (res = res());
                }
            });
            res = (res == null || res == obj ? all : res) + "";
            return res;
        },
        fill = function (str, obj) {
            return String(str).replace(tokenRegex, function (all, key) {
                return replacer(all, key, obj);
            });
        };
    Raphael.fn.popup = function (X, Y, set, pos, ret) {
        pos = String(pos || "top-middle").split("-");
        pos[1] = pos[1] || "middle";
        var r = 5,
            bb = set.getBBox(),
            w = Math.round(bb.width),
            h = Math.round(bb.height),
            x = Math.round(bb.x) - r,
            y = Math.round(bb.y) - r,
            gap = Math.min(h / 2, w / 2, 10),
            shapes = {
                top: "M{x},{y}h{w4},{w4},{w4},{w4}a{r},{r},0,0,1,{r},{r}v{h4},{h4},{h4},{h4}a{r},{r},0,0,1,-{r},{r}l-{right},0-{gap},{gap}-{gap}-{gap}-{left},0a{r},{r},0,0,1-{r}-{r}v-{h4}-{h4}-{h4}-{h4}a{r},{r},0,0,1,{r}-{r}z",
                bottom: "M{x},{y}l{left},0,{gap}-{gap},{gap},{gap},{right},0a{r},{r},0,0,1,{r},{r}v{h4},{h4},{h4},{h4}a{r},{r},0,0,1,-{r},{r}h-{w4}-{w4}-{w4}-{w4}a{r},{r},0,0,1-{r}-{r}v-{h4}-{h4}-{h4}-{h4}a{r},{r},0,0,1,{r}-{r}z",
                right: "M{x},{y}h{w4},{w4},{w4},{w4}a{r},{r},0,0,1,{r},{r}v{h4},{h4},{h4},{h4}a{r},{r},0,0,1,-{r},{r}h-{w4}-{w4}-{w4}-{w4}a{r},{r},0,0,1-{r}-{r}l0-{bottom}-{gap}-{gap},{gap}-{gap},0-{top}a{r},{r},0,0,1,{r}-{r}z",
                left: "M{x},{y}h{w4},{w4},{w4},{w4}a{r},{r},0,0,1,{r},{r}l0,{top},{gap},{gap}-{gap},{gap},0,{bottom}a{r},{r},0,0,1,-{r},{r}h-{w4}-{w4}-{w4}-{w4}a{r},{r},0,0,1-{r}-{r}v-{h4}-{h4}-{h4}-{h4}a{r},{r},0,0,1,{r}-{r}z"
            },
            offset = {
                hx0: X - (x + r + w - gap * 2),
                hx1: X - (x + r + w / 2 - gap),
                hx2: X - (x + r + gap),
                vhy: Y - (y + r + h + r + gap),
                "^hy": Y - (y - gap)

            },
            mask = [{
                x: x + r,
                y: y,
                w: w,
                w4: w / 4,
                h4: h / 4,
                right: 0,
                left: w - gap * 2,
                bottom: 0,
                top: h - gap * 2,
                r: r,
                h: h,
                gap: gap
            }, {
                x: x + r,
                y: y,
                w: w,
                w4: w / 4,
                h4: h / 4,
                left: w / 2 - gap,
                right: w / 2 - gap,
                top: h / 2 - gap,
                bottom: h / 2 - gap,
                r: r,
                h: h,
                gap: gap
            }, {
                x: x + r,
                y: y,
                w: w,
                w4: w / 4,
                h4: h / 4,
                left: 0,
                right: w - gap * 2,
                top: 0,
                bottom: h - gap * 2,
                r: r,
                h: h,
                gap: gap
            }][pos[1] == "middle" ? 1 : (pos[1] == "top" || pos[1] == "left") * 2];
        var dx = 0,
            dy = 0,
            out = this.path(fill(shapes[pos[0]], mask)).insertBefore(set);
        switch (pos[0]) {
            case "top":
                dx = X - (x + r + mask.left + gap);
                dy = Y - (y + r + h + r + gap);
                break;
            case "bottom":
                dx = X - (x + r + mask.left + gap);
                dy = Y - (y - gap);
                break;
            case "left":
                dx = X - (x + r + w + r + gap);
                dy = Y - (y + r + mask.top + gap);
                break;
            case "right":
                dx = X - (x - gap);
                dy = Y - (y + r + mask.top + gap);
                break;
        }
        out.translate(dx, dy);
        if (ret) {
            ret = out.attr("path");
            out.remove();
            return {
                path: ret,
                dx: dx,
                dy: dy
            };
        }
        set.translate(dx, dy);
        return out;
    };
})();

; (function ($) {
    rad = Math.PI / 180;
    $.fn.Pie = function (dataArray, total) {
        $(this).Doughnut(dataArray, total,0.001);
    }


    $.fn.Doughnut = function (dataArray, total, thickness) {
        var squareEdge = ($(this).height() <= $(this).width() ? $(this).height() : $(this).width()),
            radius = squareEdge / 2,
            doughnutRadius = radius - 1,
            innerRadius = doughnutRadius * (thickness || 0.65)
        canvas = Raphael($(this).get(0), squareEdge, squareEdge),
        startAngle = 0,
        internalTotal = 0;

        
        $.each(dataArray, function (index, object) {
            var value = object.value;
            internalTotal += value;

            if (internalTotal === total && dataArray.length == 1)
                value -= 0.01;

            var theta = 360 / total * value,
            random = Math.random();
            var p = doughnutSection(canvas, radius, radius, doughnutRadius, innerRadius, startAngle, startAngle + theta, { fill: "90-" + Raphael.hsb(random, 1, .8) + "-" + Raphael.hsb(random, .6, .8), stroke: "#fff", "stroke-width": 1 })
            startAngle += theta;
        });
        if (internalTotal !== total) {
            var theta = 360 / total * (total - internalTotal);
            if (theta == 360)
                theta = 359.5
            var random = Math.random();
            var p = doughnutSection(canvas, radius, radius, doughnutRadius, innerRadius, startAngle, startAngle + theta, { fill: "90-" + Raphael.rgb(50, 50, 50) + "-" + Raphael.rgb(110, 110, 110), stroke: "#fff", "stroke-width": 1 })
        }
        if (dataArray.length == 1){
            var text = Math.round(((internalTotal/total)*100))+"%";
            canvas.text(radius, radius, text).attr({ 'font-size': (innerRadius*2 / 3), 'font-family': 'Tahoma, Trebuchet-MS' });
        }
    }

    function doughnutSection(canvas, cx, cy, r, r2, startAngle, endAngle, params) {
        var x1 = cx + r * Math.cos(-startAngle * rad),
            x2 = cx + r * Math.cos(-endAngle * rad),
            y1 = cy + r * Math.sin(-startAngle * rad),
            y2 = cy + r * Math.sin(-endAngle * rad),
            xx1 = cx + r2 * Math.cos(-startAngle * rad),
            xx2 = cx + r2 * Math.cos(-endAngle * rad),
            yy1 = cy + r2 * Math.sin(-startAngle * rad),
            yy2 = cy + r2 * Math.sin(-endAngle * rad);
        return canvas.path(["M", xx1, yy1, "L", x1, y1, "A", r, r, 0, +(endAngle - startAngle > 180), 0, x2, y2, "L", xx2, yy2, "A", r2, r2, 0, +(endAngle - startAngle > 180), 1, xx1, yy1, "z"]).attr(params);
    }

    $.fn.BarChart = function (data) {
        var values = [];
        $.each(data, function (index, object) {
            values.push(object.value);
        });

        var width = $(this).width(),
            height = $(this).height(),
            paddingLeft = 20,
            paddingTop = 20,
            paddingBottom = 20,
            colorhue = .6 || Math.random(),
            color = "hsl(" + [colorhue, .5, .5] + ")",
            canvas = Raphael($(this).get(0), width, height),
            X = (width - paddingLeft) / data.length,
            max = Math.max.apply(Math, values),
            Y = (height - paddingBottom - paddingTop) / max,
            actualWidth = width - paddingLeft - X,
            actualHeight = height - paddingTop - paddingBottom,
            startX = paddingLeft + X * .5 + .5,
            startY = paddingTop + .5,
            barWidth = ((actualWidth - 4) - data.length) / data.length;
        canvas.path(DrawGrid(startX, startY, actualWidth, actualHeight, 10, 10, "#000", GridType.AxisOnly)).attr({ stroke: color, "stroke-width": 1 }).translate(0.5, 0.5)
        startX += (barWidth / 2) + 2
        $.each(data, function (index, object) {
            random = Math.random();
            canvas.path(["M", startX, height - paddingTop - (object.value * (actualHeight / max)), "V", actualHeight + startY]).attr({ stroke: Raphael.hsb(random, .6, .8), "stroke-width": barWidth });
            startX += (barWidth + 1);
        })

    }

    function DrawGrid(x, y, w, h, wv, hv, color, axisType) {
        var path = ["M", Math.round(x) + .5, Math.round(y) + .5, "L", Math.round(x) + .5, Math.round(y + h) + .5,
                                                                      Math.round(x + w) + .5, Math.round(y + h) + .5,
        ];
        if (axisType == GridType.AxisOnly)
            return path.join(",");

        path = path.concat([Math.round(x + w) + .5, Math.round(y) + .5, Math.round(x) + .5, Math.round(y) + .5]);

        var rowHeight = h / hv,
	    columnWidth = w / wv;
        for (var i = 1; i < hv; i++) {
            path = path.concat(["M", Math.round(x) + .5, Math.round(y + i * rowHeight) + .5, "H", Math.round(x + w) + .5]);
        }
        for (i = 1; i < wv; i++) {
            path = path.concat(["M", Math.round(x + i * columnWidth) + .5, Math.round(y) + .5, "V", Math.round(y + h) + .5]);
        }
        return path.join(",");
    }

    $.fn.Analytics = function (data) {
        function getAnchors(p1x, p1y, p2x, p2y, p3x, p3y) {
            var l1 = (p2x - p1x) / 2,
                l2 = (p3x - p2x) / 2,
                a = Math.atan((p2x - p1x) / Math.abs(p2y - p1y)),
                b = Math.atan((p3x - p2x) / Math.abs(p2y - p3y));
            a = p1y < p2y ? Math.PI - a : a;
            b = p3y < p2y ? Math.PI - b : b;
            var alpha = Math.PI / 2 - ((a + b) % (Math.PI * 2)) / 2,
                dx1 = l1 * Math.sin(alpha + a),
                dy1 = l1 * Math.cos(alpha + a),
                dx2 = l2 * Math.sin(alpha + b),
                dy2 = l2 * Math.cos(alpha + b);
            return {
                x1: p2x - dx1,
                y1: p2y + dy1,
                x2: p2x + dx2,
                y2: p2y + dy2
            };
        }

        var values = [];
        $.each(data, function (index, object) {
            values.push(object.value);
        });

        var width = $(this).width(),
            height = $(this).height(),
            paddingLeft = 20,
            paddingTop = 20,
            paddingBottom = 20,
            colorhue = Math.random(),
            color = "hsl(" + [colorhue, .5, .5] + ")",
            canvas = Raphael($(this).get(0), width, height),
            X = (width - paddingLeft) / (data.length - 1),
            max = Math.max.apply(Math, values),
            Y = (height - paddingBottom - paddingTop) / max,
            actualWidth = width - paddingLeft,
            actualHeight = height - paddingTop - paddingBottom,
            startX = paddingLeft * .5,
            startY = paddingTop + .5,
            txt = { font: '12px Helvetica, Arial', fill: "#fff" },
            txt1 = { font: '10px Helvetica, Arial', fill: "#fff" },
            barWidth = ((actualWidth - 4) - data.length) / data.length;
        canvas.path(DrawGrid(startX, startY, actualWidth, actualHeight, 10, 10, "#000", GridType.Grid)).attr({ stroke: "rgb(160,160,160)", "stroke-width": 1 }).translate(0.5, 0.5);
        var path = canvas.path().attr({ stroke: color, "stroke-width": 4, "stroke-linejoin": "round" }),
            bgp = canvas.path().attr({ stroke: "none", opacity: .3, fill: color });
        var labels = canvas.set()
        labels.push(canvas.text(60, 12, "Test").attr(txt));
        labels.hide();
        var frame = canvas.popup(100, 100, labels, "right").attr({ fill: "#000", stroke: "#666", "stroke-width": 2, "fill-opacity": .7 }).hide();


        var p, bgpp, points = canvas.set();
        for (var i = 0, ii = values.length; i < ii; i++) {
            var y = Math.round(height - paddingBottom - Y * values[i]);
            var x = Math.round(startX + 0.5 + (X * i));
            if (!i) {
                p = ["M", x, y, "C", x, y];
                bgpp = ["M", startX, height - paddingBottom + 1, "L", x, y, "C", x, y];
            }
            if (i && i < ii - 1) {
                var Y0 = Math.round(height - paddingBottom - Y * values[i - 1]),
                    X0 = Math.round(startX + X * (i - .5)),
                    Y2 = Math.round(height - paddingBottom - Y * values[i + 1]),
                    X2 = Math.round(startX + X * (i + .5));
                var a = getAnchors(X0, Y0, x, y, X2, Y2);
                p = p.concat([a.x1, a.y1, x, y, a.x2, a.y2]);
                bgpp = bgpp.concat([a.x1, a.y1, x, y, a.x2, a.y2]);
            }
            var dot = canvas.circle(x, y, 4).attr({ fill: "#FFF", stroke: "hsl(" + [colorhue, .7, .7] + ")", "stroke-width": 2 });
            points.push(dot);
            var rect = points[points.length - 1];
            (function(posX,posY,dataText,dot){
                dot.hover(function () {
                    var side = "right";
                    if (posX + frame.getBBox().width > width) {
                        side = "left";
                    }
                    labels[0].attr({ text: dataText });
                    var tempPopup = canvas.popup(posX, posY, labels, side, 1);
                    lx = labels[0].transform()[0][1] + tempPopup.dx;
                    ly = labels[0].transform()[0][2] + tempPopup.dy;
                    labels.attr({ transform: ["t", lx, ly] });
                    frame.attr({ path: tempPopup.path, transform: ["t", tempPopup.dx, tempPopup.dy] });
                    this.attr("r", 6);
                    frame.show();
                    labels[0].show();
                },function (){
                    this.attr("r", 4);
                    labels[0].hide();
                    frame.hide();
                });
            })(x, y, data[i].name, dot);
        }
        
        p = p.concat([x, y, x, y]);
        bgpp = bgpp.concat([x, y, x, y, "L", x, height - paddingBottom + 1, "z"]);
        path.attr({ path: p });
        bgp.attr({ path: bgpp });
        points.toFront();
        labels.toFront();
    }



}(jQuery));

var GridType = {
    AxisOnly: 0,
    Grid: 1
};
