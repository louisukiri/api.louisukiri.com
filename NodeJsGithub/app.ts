/// <reference path="./node_definitions/node.d.ts" />
/// <reference path="./node_definitions/express3.d.ts" />

var express = require('express');
var app = express();

app.set('port', process.env.PORT || 3000);
app.get('/', function (req, res) {
    res.type('text/plain');
    res.send('solid home page');
});

app.get('/about', function (req, res) {
    res.type('text/plain');
    res.send('my name is Lou-to-the-Is');
});

//custom 404 page
app.use(function (req, res) {
    res.type('text/plain');
    res.status(404);
    res.send('404 - Not Found');
});

//custom 500 page
app.use(function (err, req, res, next) {
    console.error(err.stack);
    res.type('text/plain');
    res.status(500);
    res.send('500 - Custom server 500 errorrr');
});


app.listen(app.get('port'), function () {
    console.log('Express started on http://localhost:' + app.get('port') + '; press Ctrl-C to terminate.');
});