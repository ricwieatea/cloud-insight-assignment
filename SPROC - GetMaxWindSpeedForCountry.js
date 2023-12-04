function GetMaxWindSpeedForCountry(country) {
    var collection = getContext().getCollection()

    var isAccepted = collection.queryDocuments(
        collection.getSelfLink(),
        'SELECT TOP 1 w.WindSpeed, w.Country FROM Weather w WHERE w.Country = "' + country + '" ORDER BY w.WindSpeed DESC',
        function (err, feed, options) {
            if ((err)) throw err;

            if (!feed || !feed.length) {
                var response = getContext().getResponse();
                response.setBody('no docs found');
            }
            else {
                var response = getContext().getResponse();
                var body = { prefix: country, feed: feed};
                response.setBody(JSON.stringify(body));
            }
        });

    if (!isAccepted) throw new Error('The query was not accepted by the server.');
}