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
        $('#chatroom').append('<p class="text-danger bg-info"><b>' + htmlEncode(message) + '</p>');        
    };
    
    // Функция, вызываемая при подключении нового пользователя
    chat.client.onConnected = function (id, userName, allUsers) {        
        // установка в скрытых полях имени и id текущего пользователя
        $('#userId').val(id);
        $('#textUserName').val(userName);
        // Добавление всех пользователей
        for (i = 0; i < allUsers.length; i++) {
            AddUser(allUsers[i].ConnectionId, allUsers[i].Name);        }
    }

    // Добавляем нового пользователя
    chat.client.onNewUserConnected = function (id, name) {

        AddUser(id, name);
    }

    // Удаляем пользователя
    chat.client.onUserDisconnected = function (id, userName) {

        $('#' + id).remove();
    }

    //подключение к группе
    $.connection.hub.start(function () {
        chat.server.join($('#textUserName').val());
    });

    // Открываем соединение
    $.connection.hub.start().done(function () {
        //$('#messagesReceive')
        chat.server.connect($('#userId').val() , $('#textUserName').val());
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

//Добавление нового пользователя
function AddUser(id, name) {

    var userId = $('#hdId').val();

    if (userId != id) {

        $("#chatusers").append('<p id="' + id + '"><b>' + name + '</b></p>');
    }
}