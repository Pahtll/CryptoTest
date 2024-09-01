import React from "react";
import "./App.css";
import Input from "./components/Input/Input";
import Header from "./components/Header/Header";
import Blank from "./components/Blank/Blank";
import MessageBox from "./components/MessageBox/MessageBox";
import { get_messages } from "./api/responses";
import { TMessage } from "./types";
import { HubConnectionBuilder } from "@microsoft/signalr";

const App = () => {
  const [messages, setMessages] = React.useState<TMessage[]>([]);

  async function fetch_messages() {
    try {
      const newMessages = await get_messages();
      setMessages(newMessages);
    } catch (error) {
      console.error("Error fetching messages:", error);
    }
  }

  const [connection, setConnection] =
    React.useState<signalR.HubConnection | null>(null);

  React.useEffect(() => {
    const newConnection = new HubConnectionBuilder()
      .withUrl("http://localhost:5290/chat")
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);

    newConnection
      .start()
      .then(() => {
        console.log("Connected to SignalR hub");
        return newConnection.invoke("JoinChatroom", {
          Username: "Bebra",
          Chatroom: "Bebra",
        });
      })
      .then(() => console.log("Joined chatroom successfully"))
      .catch((err) =>
        console.error("Error connecting to hub or joining chatroom:", err)
      );

    newConnection.on("ReceiveMessage", (username, message) => {
      fetch_messages();
    });

    return () => {
      newConnection.stop();
    };
  }, []);

  React.useEffect(() => {
    fetch_messages();
  }, []);

  const formatDate = (timestamp: string) => {
    const date = new Date(timestamp);
    const day = date.getDate().toString().padStart(2, "0");
    const month = (date.getMonth() + 1).toString().padStart(2, "0");
    const year = date.getFullYear();
    return `${day}.${month}.${year}`;
  };

  const groupMessagesByDate = (messages: TMessage[]) => {
    const groupedMessages: { [key: string]: TMessage[] } = {};
    messages.forEach((message) => {
      const date = formatDate(message.sentAt);
      if (!groupedMessages[date]) {
        groupedMessages[date] = [];
      }
      groupedMessages[date].push(message);
    });
    return groupedMessages;
  };

  const groupedMessages = groupMessagesByDate(messages);

  return (
    <div className="App">
      <Header>
        <h1>:3</h1>
      </Header>
      <div className="mainContainer">
        <Blank>
          {Object.keys(groupedMessages)
            .reverse()
            .map((date) => (
              <>
                {groupedMessages[date].reverse().map((message, index) => (
                  <MessageBox key={index} messageTime={message.sentAt}>
                    {message.text}
                  </MessageBox>
                ))}
                <div className="dateSeparator">{date}</div>
              </>
            ))}
        </Blank>
        <div className="inputContainer">
          <Input connection={connection} />
        </div>
      </div>
    </div>
  );
};

export default App;
