$(function () {

    // Ссылка на автоматически-сгенерированный прокси хаба
    var chat = $.connection.ChatHub;

    var datetime = new Date().toLocaleTimeString().replace(/.*(\d{2}:\d{2}:\d{2}).*/, "$1");
    var dateyear = new Date().toLocaleDateString();

    
    // Объявление функции, которая вызывает хаб при получении сообщений
    chat.client.addMessage = function (message) {
        
        // Добавление сообщений на веб-страницу         
        $('#chatroom').prepend('<div class="text-success bg-success"><div style="text-align:right"><b>' + message.senderemail + '</div><div style="color:blue; text-align:right"> (' + message.createdate + ') </div><br/>' + message.mess + '</div><hr/>');

        var count = +$('#cell').text();
        $('#cell').text(++count);
        chat.server.messageReadDate(datetime, $('#userId').val());
    };
    chat.client.myMessage = function (message) {
        // Добавление сообщений на веб-страницу   
        $('#chatroom').prepend('<div class="text-danger bg-info"><span><b>' + htmlEncode(message.forWho) + ' (' + dateyear + ') ' + datetime + '</span><br/>' + htmlEncode(message.mess) + '</div><hr color="black"/>');
    };

    chat.client.onConnected = function (message) {
        // Добавление сообщений на веб-страницу        
        $('#chatroom').prepend('<span style="text-align:left">' + message.creatoremail + '</span>' + '<span style="color:blue; text-align:right"> (' + message.createdate + ') </span><br/><span>' + message.mess + '</span><hr/>');
    };

    //подключение к группе
    $.connection.hub.start(function () {
        chat.server.join($('#textUserName').val());
    });

    // Открываем соединение
    $.connection.hub.start().done(function () {
        chat.server.onConnected($('#userId').val());

        //Отправка по нажатии комбинации клавиш
        $('#message').keydown(function (e)
        {
            if ((e.keyCode === 10 || e.keyCode === 13) && e.ctrlKey)
            {
                // Ctrl-Enter pressed
                var message = $("#message").val();
                if (message.length > 0)
                {                    
                    chat.server.saveToDb(
                        {
                            // Вызываем у хаба метод SaveToDb
                            Msg: $('#message').val(),
                            Group: $('#textUserName').val(),
                            SelectedUsers: selectedInUsers,
                            SenderId: $('#userId').val(),
                            SenderName: $('#textUserName').val(),
                            CreateDate: dateyear,
                            CreateTime: datetime
                        });

                    // Вызываем у хаба метод Send
                    chat.server.send(
                        {
                            Msg: $('#message').val(),                     
                            Group: $('#textUserName').val(),
                            SelectedUsers: selectedInUsers,
                            SenderId: $('#userId').val(),
                            SenderName: $('#textUserName').val(),
                            CreateDate: dateyear,
                            CreateTime: datetime
                        }
                    );
                }
                $('#message').val('');
                $('#cell').text('0');
            }            
        }),        

        //Отправка по нажатию кнопки Send
        $('#sendmessage').click(function () {            
            var message = $("#message").val();
            if (message.length > 0)
            {
                // Вызываем у хаба метод SaveToDb
              chat.server.saveToDb(
                {
                    Msg: $('#message').val(),
                    Group: $('#textUserName').val(),
                    SelectedUsers: selectedInUsers,
                    SenderId: $('#userId').val(),
                    SenderName: $('#textUserName').val(),
                    CreateDate: dateyear,
                    CreateTime: datetime
                });

              // Вызываем у хаба метод Send
              chat.server.send(
                {
                    Msg: $('#message').val(),                     
                    Group: $('#textUserName').val(),
                    SelectedUsers: selectedInUsers,
                    SenderId: $('#userId').val(),
                    SenderName: $('#textUserName').val(),
                    CreateDate: dateyear,
                    CreateTime: datetime
                });
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
