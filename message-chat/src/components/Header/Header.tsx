import React, { ReactNode } from "react";
import "./styles.css";

interface HeaderProps {
  children?: ReactNode;
}

const Header: React.FC<HeaderProps> = ({ children }) => {
  return <div className="header">{children}</div>;
};

export default Header;
