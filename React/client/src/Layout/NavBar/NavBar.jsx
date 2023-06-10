import React from 'react';
import './NavBar.css';
import { Link } from 'react-router-dom';

const Home = () => {
  return (
   <nav className="NavBar-Wrapper">
     <div>
       <h3 className="NavBar-Title">Mern Stack - Crud App</h3>
     </div>
     <div className="NavBar-Links">
      <Link to="/" className="NavBar-Link">Home</Link>
      <Link to="/addStudent" className="NavBar-Link">Add</Link>
      <a href="https://localhost:7044" className="NavBar-Link">Go to JobMatch</a>
     </div>
   </nav>
  );
};

export default Home;
