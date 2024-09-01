import React, { useState } from "react";
import { IoSend } from "react-icons/io5";
import SubmitButton from "../SubmitButton/SubmitButton";
import "./styles.css";

interface InputProps {
  connection: signalR.HubConnection | null;
}

const Input: React.FC<InputProps> = ({ connection }) => {
  const [text, setText] = useState<string>("");

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    if (text.trim() !== "" && connection) {
      try {
        await connection.invoke("SendMessage", text);
        setText("");
      } catch (error) {
        alert(error);
      }
    }
  };

  const handleKeyPress = (event: React.KeyboardEvent) => {
    if (event.key === "Enter" && !event.shiftKey) {
      event.preventDefault();
      handleSubmit(event as unknown as React.FormEvent<HTMLFormElement>);
    }
  };

  return (
    <form className="form" onSubmit={handleSubmit}>
      <textarea
        placeholder="Введите сообщение"
        value={text}
        className="PromptInput"
        onChange={(e) => setText(e.target.value)}
        onKeyDown={handleKeyPress}
        rows={1}
      />
      <SubmitButton disabled={!text.trim()}>
        <IoSend />
      </SubmitButton>
    </form>
  );
};

export default Input;