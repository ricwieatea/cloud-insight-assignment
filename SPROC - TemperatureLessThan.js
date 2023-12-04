function TemperatureLessThan(temperature) {
    var collection = getContext().getCollection()

    var isAccepted = collection.queryDocuments(
        collection.getSelfLink(),
        'SELECT w.Temperature, w.WindSpeed, w.Country FROM Weather w WHERE w.Temperature < ' + temperature ,
        function (err, feed, options) {
            if ((err)) throw err;

            if (!feed || !feed.length) {
                var response = getContext().getResponse();
                response.setBody('no docs found');
            }
            else {
                var response = getContext().getResponse();
                var body = { prefix: temperature, feed: feed};
                response.setBody(JSON.stringify(body));
            }
        });

    if (!isAccepted) throw new Error('The query was not accepted by the server.');
}