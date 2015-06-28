var Dijkstra = function (graph) {
    var inner =
    {
        graph: graph
    }

    this.calculate = function (start) {
        var parent = {};
        var distTo = {},
            marked = [];

        var nextU = -1, nextV = -1, indexFrom = -1, indexTo = -1, minDistance = Infinity, tempDistance = 0;
        for (var i = 0; i < inner.graph.vertices.length; i++) {
            distTo[i] = Infinity;
        }

        for (var i = 0; i < inner.graph.edges.length; i++) {
            indexFrom = inner.graph.edges[i].indexFrom;
            indexTo = inner.graph.edges[i].indexTo;
            if (indexFrom == start || indexTo == start) {
                nextU = indexFrom == start ? indexTo : indexFrom;
                distTo[nextU] = inner.graph.edges[i].length;
                parent[nextU] = start;
            }
        }

        distTo[start] = 0;
        marked.push(start);
        while (marked.length < inner.graph.vertices.length) {
            nextU = -1;
            minDistance = Infinity;
            for (var i = 0; i < inner.graph.vertices.length; i++) {
                if (distTo[i] < minDistance && marked.indexOf(i) == -1) {
                    nextU = i;
                    minDistance = distTo[i];
                }
            }

            marked.push(nextU);
            for (var i = 0; i < inner.graph.edges.length; i++) {
                indexFrom = inner.graph.edges[i].indexFrom;
                indexTo = inner.graph.edges[i].indexTo;
                if (indexFrom != nextU && indexTo != nextU) continue;
                nextV = indexFrom == nextU ? indexTo : indexFrom;
                if (marked.indexOf(nextV) != -1) continue;

                tempDistance = distTo[nextU] + inner.graph.edges[i].length;
                if (tempDistance < distTo[nextV]) {
                    distTo[nextV] = tempDistance;
                    parent[nextV] = nextU;
                }
            }
        }

        return parent;
    };
};