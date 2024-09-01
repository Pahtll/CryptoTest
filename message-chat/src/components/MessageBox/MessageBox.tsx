import React, { ReactNode } from "react";
import "./styles.css";

interface MessageBoxProps {
  children: ReactNode;
  messageTime: string; // Добавляем пропс для времени
}

const MessageBox: React.FC<MessageBoxProps> = ({ children, messageTime }) => {
  // Функция для форматирования времени
  const formatTimestamp = (timestamp: string) => {
    const date = new Date(timestamp);
    const hours = date.getHours().toString().padStart(2, "0");
    const minutes = date.getMinutes().toString().padStart(2, "0");
    return `${hours}:${minutes}`;
  };

  return (
    <div className="messageContainer">
      <div className="messageTimestamp">{formatTimestamp(messageTime)}</div>
      <div className="messageBox">{children}</div>
    </div>
  );
};

export default MessageBox;
