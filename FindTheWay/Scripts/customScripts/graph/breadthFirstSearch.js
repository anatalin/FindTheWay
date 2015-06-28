var BreadthFirstSearch = function (graph) {
    var inner =
    {
        graph: graph
    }

    this.calculate = function (start) {
        var parent = {};
        var queue = [start], 
            marked = [];

        for (var v = start; queue.length > 0;) {
            v = queue.shift();
            marked.push(v);
            inner.graph.edges
                .filter(function(e) {
                    return e.indexFrom == v || e.indexTo == v;
                })
                .map(function(e) {
                    return e.indexFrom == v ? e.indexTo : e.indexFrom;
                })
                .forEach(function (av) {
                    if (marked.indexOf(av) == -1) {
                        queue.push(av);
                        parent[av] = v;
                        marked.push(av);
                    }
                });
        }

        return parent;
    };
};