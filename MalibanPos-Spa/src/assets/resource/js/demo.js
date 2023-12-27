/* export function test1(receiptDetails) {

    console.log('Calling test 1 function' + JSON.stringify(receiptDetails));
} */

function test1(receiptPrint) {

    var ws = new WebSocket("ws://localhost:5010");

    ws.onopen = function() {
        ws.send(JSON.stringify(receiptPrint));
        // alert("Message sent!");
    };


    ws.onmessage = function(evt) {
        //alert("About to receive data");

        // textarea.value += 'Wait to receive data' + newline;
        var received_msg = evt.data;
        //  textarea.value += "Message received = " + received_msg + newline;

        // alert("Message received = " + received_msg);
    };

    ws.onclose = function() {
        // websocket is closed.

        //  textarea.value += "Connection is closed..." + newline;
        // alert("Connection is closed...");
    };
};