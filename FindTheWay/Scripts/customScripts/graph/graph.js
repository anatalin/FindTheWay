function Graph(height, width, vertexRadius, verticesCount) {
    this.height = height;
    this.width = width;
    this.vertices = [];
    this.edges = [];
    var xRange = this.width - vertexRadius * 2;
    var yRange = this.height - vertexRadius * 2;
    verticesCount = verticesCount || 0;

    var r = xRange / 2;
    var deltaPhi = 2 * Math.PI / verticesCount;
    for (var i = 0, phi = 0; i < verticesCount; i++, phi += deltaPhi) {
        var nextR = r - Math.random() * vertexRadius;
        this.vertices.push({
            x: (nextR * Math.cos(phi) + xRange) / 2,
            y: (nextR * Math.sin(phi) + yRange) / 2
        });
    }

    this.addVertex = function () {
        this.vertices.push({
            x: Math.floor(Math.random() * xRange + vertexRadius),
            y: Math.floor(Math.random() * yRange + vertexRadius)
        });
    };

    this.addEdge = function (indexFrom, indexTo) {
        if (indexTo < 0 || indexTo >= this.vertices.length ||
            indexFrom < 0 || indexFrom >= this.vertices.length) {
            return;
        }

        this.edges.push({
            indexFrom: indexFrom,
            indexTo: indexTo,
            length: Math.floor(
                CalculateFunctions.eucledianDistance(
                    this.vertices[indexFrom].x, this.vertices[indexTo].x,
                    this.vertices[indexFrom].y, this.vertices[indexTo].y)
            )
        });
    };

    this.addRandomEdges = function (count) {
        var index = -1;
        for (var i = 0; i < count; i++) {
            for (var j = 0; j < 2; j++) {
                index = i;
                while (index == i) {
                    index = Math.floor(Math.random() * verticesCount);
                }

                this.addEdge(i, index);
            }
        }
    };
}