var mjmlModule = require('mjml');

module.exports = function (callback, body) {

    try {
        var htmlOutput = mjmlModule(body,
            {
                minify: true
            });

        if (htmlOutput && htmlOutput.errors && htmlOutput.errors.length) {
            console.error(htmlOutput.errors.join('\n'));
        }

        callback(null, htmlOutput.html);

    } catch (e) {
        callback(e, body);
    }
};