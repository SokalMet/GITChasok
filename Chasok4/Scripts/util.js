$(function () {

    // Ссылка на автоматически-сгенерированный прокси хаба
    var chat = $.connection.ChatHub;
    // Объявление функции, которая вызывает хаб при получении сообщений
    chat.client.addMessage = function (message) {
        // Добавление сообщений на веб-страницу         
        $('#chatroom').append('<p><b>' + htmlEncode(message) + '</p>');
        
        var count = +$('#cell').text();
        $('#cell').text(++count);
        chat.server.messageReceived(messageId);
    };
    chat.client.myMessage = function (message) {
        // Добавление сообщений на веб-страницу         
        $('#chatroom').append('<p class="text-danger"><b>' + htmlEncode(message) + '</p>');        
    };
    

    //подключение к группе
    $.connection.hub.start(function () {
        chat.server.join($('#textUserName').val());
    });

    // Открываем соединение
    $.connection.hub.start().done(function () {
        //$('#messagesReceive')

        $('#sendmessage').click(function () {
            // Вызываем у хаба метод Send
            chat.server.send(
                {
                    Msg: $('#message').val(),                     
                    Group: $('#textUserName').val(),
                    SelectedUsers: selectedInUsers,
                    SenderId: $('#userId').val(),
                    SenderName: $('#textUserName').val()
                }
            );
            $('#message').val('');
            $('#cell').text('0');
        });
    });
});

// Кодирование тегов
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}
