
$(function () {
    
    // Ссылка на автоматически-сгенерированный прокси хаба
    var chat = $.connection.ChatHub;
    $.connection.hub.logging = true;

    $.connection.hub.error(function (error) {
        console.log('SignalR my error: ' + error)
    });

    //var datetime = new Date().toLocaleTimeString().replace(/.*(\d{2}:\d{2}:\d{2}).*/, "$1");
    //var dateyear = new Date().toLocaleDateString();

    //подключение к группе
    $.connection.hub.start(function () {
        chat.server.join($('#textUserName').val());
    });
    chat.server.logging = true;


    // Открываем соединение
    $.connection.hub.start().done(function () {
        
        chat.server.onConnected($('#userId').val());
        //debugger;

        function myfunction() {
            var message = $("#message").val();

            if (message.length > 0) {
                //debugger;
                var date = new Date;
                // Вызываем у хаба метод SaveToDb
                chat.server.saveToDb(message, $('#userId').val(), $('#textUserName').val(), selectedInUsers, date);
                  
                // Вызываем у хаба метод Send
                chat.server.send(message, $('#userId').val(), $('#textUserName').val(), selectedInUsers, date);                 
            }
        };

        //Отправка по нажатии комбинации клавиш
        $('#message').keydown(function (e)
        {
            if ((e.keyCode === 10 || e.keyCode === 13) && e.ctrlKey)
            {
                myfunction();

                $('#message').val('');
                $('#cell').text('0');
            }            
        }),        

        //Отправка по нажатию кнопки Send
        $('#sendmessage').click(function () {

            myfunction();

            $('#message').val('');
            $('#message').focus();
            $('#cell').text('0');
        });
    });


    // Объявление функции, которая вызывает хаб при получении сообщений
    chat.client.addMessage = function (userName, mess, creationTime) {

        // Добавление сообщений на веб-страницу         
        $('#chatroom').prepend('<div class="text-success bg-success"><div style="text-align:right"><b>' + htmlEncode(userName) + '</div><div style="color:blue; text-align:right"> Today at ' + creationTime + '</div>' + htmlEncode(mess) + '</div><br/>');

        var count = +$('#cell').text();
        $('#cell').text(++count);
        var date = new Date;
        chat.server.messageReadDate(date, $('#userId').val());
    };

    chat.client.myMessage = function (mess, forMe, creationTime) {
        // Добавление сообщений на веб-страницу   
        $('#chatroom').prepend('<div class="text-danger bg-info"><span><b>' + htmlEncode(forMe) + '<span style="color:blue;">Today at ' + creationTime + '</span></span><br/>' + htmlEncode(mess) + '</div><br/>');
    };


    chat.client.onConnected = function (message) {
        // Добавление сообщений на веб-страницу  
        var time = moment(message.createdate).calendar();

        $('#chatroom').prepend('<span style="color:green; text-align:left">' + message.creatoremail + '</span>' + '<span style="color:blue; text-align:right"> (' + time + ') </span><br/><span>' + message.mess + '</span><br/><br/>');
    };

});

// Кодирование тегов
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}
