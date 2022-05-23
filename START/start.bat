start "mq_stock" /MIN ..\rabbitmq_server-3.10.1\sbin\rabbitmq-server.bat ^> utils\mq.txt

call py utils\rabbit.py

START "server_stock" /MIN ..\Predictor\predictor_start.bat

call py utils\server.py

cd ..\Gui\GuiPZ\GuiPZ\bin\Release\net6.0-windows
CALL GuiPZ.exe

taskkill /FI "WindowTitle eq mq_stock*" /T /F
taskkill /FI "WindowTitle eq server_stock*" /T /F

cd ..\..\..\..\..\..\
del START\utils\mq.txt
del START\utils\server.txt









