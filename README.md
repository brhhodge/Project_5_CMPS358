# Project_5_CMPS3458
  Project Description Create a computing system that can be operated as above. The system will consists of a server running in a console application that will wait for a client    (also running as a console application) to connect and, when it connects, will allow messages to be exchanged. The exchange will be in “lock step” with the client sending      its user’s message first, the server then displaying the received message. The server’s user can then transmit a message to the client, which the client will then display.     At this point, the client’s user can transmit another message, etc.

Design Requirements
  • The users of the server and client must enter the password / key on program start. The messages
  transmitted will be encrypted and decrypted based on the password / key entered.
  • Ideally, the client’s user would also be required to enter the URL or IP address of the server and its
  port, but running both on the same computer and use of “localhost” with a fixed port (Example:
  8081) is fine for this project.
  • The exchange of messages will continue until one of the users enters Ctrl-c. The other user will see
  a message indicating the session is over.

Tips and Suggestions
  • Use of a try-catch is one way to detect the exception thrown by the loss of connection.
  • For the basic client and server, adapt the “SimpleClient” and “SimpleServer” examples provided in
  “SimpleClientServer.zip” (and as demonstrated in class) to transmit messages in both directions. To
  do this, both client and server will need a BinaryWriter object and a BinaryReader object. The client
  must transmit first and receive second. The server must receive first and transmit second.
  • Adapt the code found in “EncryptionApp” as provided in “CryptographyExamples.zip” (and as
  demonstrated in class) to perform the encryption and decryption.
  • Make sure messages are correctly passed before in both directions before adding the encryption and
  decryption.
