var AStar = function (graph) {
    var inner =
    {
        graph: graph
    }

    function d(v1, v2) {
        var edge = inner.graph.edges.filter(function (e) {
            return (e.indexFrom == v1 && e.indexTo == v2) ||
            (e.indexFrom == v2 && e.indexTo == v1);
        });

        return edge.length > 0 ? edge[0].length : Infinity;
    }

    this.calculate = function (start, target) {
        var closed = [], open = [], g = [], f = [];
        var parent = {};
        var x = 0, tmp = 0;
        var isBetter = false;
        for (var i = 0; i < inner.graph.vertices.length; i++) {
            g[i] = d(start, i);
            f[i] = 0;
        }

        g[start] = 0;
        var distStartFinish = CalculateFunctions.eucledianDistance(
            inner.graph.vertices[start].x, inner.graph.vertices[target].x,
            inner.graph.vertices[start].y, inner.graph.vertices[target].y);
        open.push({
            vertexIndex: start,
            distanceFromFinish: distStartFinish
        });

        f[start] = g[start] + distStartFinish;

        while (open.length > 0) {
            open.sort(function (v1, v2) {
                return (g[v1.vertexIndex] + v1.distanceFromFinish) -
                    (g[v2] + v2.distanceFromFinish);
            });

            x = (open.shift()).vertexIndex;
            if (x == target) {
                // found the shortest path
                var pathBack = [];
                for (var i = target; i !== undefined; i = parent[i]) {
                    pathBack.push(i);
                }

                return pathBack.reverse();
            }

            closed.push(x);
            for (var i = 0; i < inner.graph.edges.length; i++) {
                if (inner.graph.edges[i].indexFrom != x && inner.graph.edges[i].indexTo != x) continue;
                var y = inner.graph.edges[i].indexTo == x ?
                    inner.graph.edges[i].indexFrom :
                    inner.graph.edges[i].indexTo;

                if (closed.indexOf(y) != -1) {
                    continue;
                }

                tmp = g[x] + d(x, y);
                distStartFinish = CalculateFunctions.eucledianDistance(
                    inner.graph.vertices[y].x, inner.graph.vertices[target].x,
                    inner.graph.vertices[y].y, inner.graph.vertices[target].y);
                if (open.filter(function(o) { return o.vertexIndex == y; }).length == 0) {
                    open.push({
                        vertexIndex: y,
                        distanceFromFinish: distStartFinish
                    });
                    isBetter = true;
                } else {
                    isBetter = tmp < g[y];
                }

                if (isBetter) {
                    parent[y] = x;
                    g[y] = tmp;
                    f[y] = g[y] + distStartFinish;
                }
            }
        }

        return {};
    };
};