import React, { ReactNode } from 'react';
import './styles.css'


interface BlankProps {
  children?: ReactNode;
}

const Blank: React.FC<BlankProps> = ({ children }) => {
  return (
    <div className="blank">
        {children}
    </div>
  );
}

export default Blank