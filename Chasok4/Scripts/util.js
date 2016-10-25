$(function () {

    //$('#chatBody').hide();
    //$('#loginBlock').show();
    // Ссылка на автоматически-сгенерированный прокси хаба
    var chat = $.connection.ChatHub;
    // Объявление функции, которая хаб вызывает при получении сообщений
    chat.client.addMessage = function (message) {
        // Добавление сообщений на веб-страницу         
        $('#chatroom').append('<p><b>' + htmlEncode(message) + '</p>');
        
    };

    // Функция, вызываемая при подключении нового пользователя
    chat.client.onConnected = function (id, userName, allUsers) {
        //$('#loginBlock').hide();
        //$('#chatBody').show();
        // установка в скрытых полях имени и id текущего пользователя
        $('#hdId').val(id);
        $('#username').val(userName);
        $('#header').html('<h3>Добро пожаловать, ' + userName + '</h3>');

        // Добавление всех пользователей
        for (i = 0; i < allUsers.length; i++) {

            AddUser(allUsers[i].ConnectionId, allUsers[i].Name);
        }
    }

    // Добавляем нового пользователя
    chat.client.onNewUserConnected = function (id, name) {

        AddUser(id, name);
    }

    // Удаляем пользователя
    chat.client.onUserDisconnected = function (id, userName) {

        $('#' + id).remove();
    }

    $.connection.hub.start(function () {
        chat.server.join("RoomA");
        
    });
    // Открываем соединение
    $.connection.hub.start().done(function () {

        $('#sendmessage').click(function () {
            // Вызываем у хаба метод Send
            chat.server.send({ Msg: $('#textUserName').val() + ": " + $('#message').val(), Group: "RoomA", SelectedUsers: selectedInUsers });
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
}