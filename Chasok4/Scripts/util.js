$(function () {

    // Ссылка на автоматически-сгенерированный прокси хаба
    var chat = $.connection.ChatHub;

    var datetime = new Date().toLocaleTimeString().replace(/.*(\d{2}:\d{2}:\d{2}).*/, "$1");
    var dateyear = new Date().toLocaleDateString();

    
    // Объявление функции, которая вызывает хаб при получении сообщений
    chat.client.addMessage = function (message) {
        var date = new Date(Date.parse(message.createdate)).toLocaleTimeString().replace(/.*(\d{2}:\d{2}:\d{2}).*/, "$1");
        // Добавление сообщений на веб-страницу         
        $('#chatroom').prepend('<div class="text-success bg-success"><div style="text-align:right"><b>' + message.senderemail + '</div><div style="color:blue; text-align:right"> (' + date + ') </div><br/>' + message.mess + '</div><hr align="center" width="300" color="Red"/>');

        var count = +$('#cell').text();
        $('#cell').text(++count);
    };
    chat.client.myMessage = function (message) {
        // Добавление сообщений на веб-страницу   
        $('#chatroom').prepend('<div class="text-danger bg-info"><span><b>' + htmlEncode(message.forWho) + ' (' + dateyear + ') ' + datetime + '</span><br/>' + htmlEncode(message.mess) + '</div><hr align="center" width="300" color="Red"/>');
    };

    chat.client.onConnected = function (message) {
        // Добавление сообщений на веб-страницу        
        $('#chatroom').prepend('<div><span>' + message.creatoremail + '<div style="color:blue; text-align:right"> (' + message.createdate + ') </div>' + '</span>' + '<span>' + message.mess + '</span></div><hr align="center" width="300" color="Red"/>');
    };

    //подключение к группе
    $.connection.hub.start(function () {
        chat.server.join($('#textUserName').val());
    });

    // Открываем соединение
    $.connection.hub.start().done(function () {
        //$('#messagesReceive')
        //chat.server.connect($('#userId').val() , $('#textUserName').val());
        chat.server.onConnected($('#userId').val());
        $('#message').keydown(function (e)
        {
            if ((e.keyCode == 10 || e.keyCode == 13) && e.ctrlKey)
            {
                // Ctrl-Enter pressed
                var date = new Date();
                var message = $("#message").val();
                if (message.length > 0)
                {
                    chat.server.saveToDb(
                        {
                            Msg: $('#message').val(),
                            Group: $('#textUserName').val(),
                            SelectedUsers: selectedInUsers,
                            SenderId: $('#userId').val(),
                            SenderName: $('#textUserName').val(),
                            CreateDate: datetime
                        });

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
                }
                $('#message').val('');
                $('#cell').text('0');
            }            
        }),        

        $('#sendmessage').click(function () {
            var date = new Date();
            var message = $("#message").val();
            if (message.length > 0)
                {
            chat.server.saveToDb(
                {
                    Msg: $('#message').val(),
                    Group: $('#textUserName').val(),
                    SelectedUsers: selectedInUsers,
                    SenderId: $('#userId').val(),
                    SenderName: $('#textUserName').val(),
                    CreateDate: datetime
                });

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
            }
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
//function AddUser(id, name) {

//    var userId = $('#hdId').val();

//    if (userId != id) {

//        $("#chatusers").append('<p id="' + id + '"><b>' + name + '</b></p>');
//    }
//}