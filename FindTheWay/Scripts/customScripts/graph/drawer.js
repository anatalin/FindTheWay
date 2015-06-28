function Drawer(svg) {
    this.svg = svg;
    this.drawVertex = function (x, y, text) {
        svg
            .append("circle")
            .attr("cx", x)
            .attr("cy", y)
            .attr("r", verticesProperties.r)
            .attr("class", "vertex");

        svg
            .append("text")
            .attr("x", x)
            .attr("y", y)
            .attr("text-anchor", "middle")
            .attr("class", "vertex-text")
            .text(text);
    }

    this.drawEdge = function (x1, y1, x2, y2, text, strokeColor) {
        strokeColor = strokeColor || '#' + Math.floor(Math.random() * 16777215).toString(16);

        svg
            .append("line")
            .attr("x1", x1)
            .attr("y1", y1)
            .attr("x2", x2)
            .attr("y2", y2)
            .attr("class", "line")
            .style("stroke", strokeColor);

        if (text) {
            svg
            .append("text")
            .attr("x", (x1 + x2) / 2)
            .attr("y", (y1 + y2) / 2)
            .attr("text-anchor", "end")
            .style("font-size", "20px")
            .style("stroke", strokeColor)
            .text(text);
        }

    }

    this.drawGraph = function (g, showWeights, edgeStrokeColor) {
        for (var i = 0; i < g.edges.length; i++) {
            var x1 = g.vertices[g.edges[i].indexFrom].x,
                y1 = g.vertices[g.edges[i].indexFrom].y,
                x2 = g.vertices[g.edges[i].indexTo].x,
                y2 = g.vertices[g.edges[i].indexTo].y;

            this.drawEdge(x1, y1, x2, y2,
                showWeights ? g.edges[i].length : null,
                edgeStrokeColor);
        }

        for (i = 0; i < g.vertices.length; i++) {
            this.drawVertex(g.vertices[i].x, g.vertices[i].y, i);
        }

    }

    this.drawPath = function (g, path, strokeColor) {
        for (var i = 0; i < path.length - 1; i++) {
            var x1 = g.vertices[path[i]].x,
                y1 = g.vertices[path[i]].y,
                x2 = g.vertices[path[i + 1]].x,
                y2 = g.vertices[path[i + 1]].y;
            this.drawEdge(x1, y1, x2, y2, null, strokeColor);
        }
    }
}