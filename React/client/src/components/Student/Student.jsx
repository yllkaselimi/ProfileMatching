import './Student.css';
import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import Avatar from 'react-avatar';


const Student = ({ _id, name, email, enrollnumber, removeStudent }) => {

  useEffect(() => {
    fetch('https://localhost:7044/api/jobpostsapi/getUserCredentials', { mode: 'cors', credentials: 'include' })
      .then(response => {
        if (response.ok) {
          return response.json(); // Parse the response as JSON
        } else if (response.status === 404) {
          window.location.href = 'https://localhost:7044/Identity/Account/Login'; // Redirect to the login page
        } else {
          throw new Error('Error fetching user credentials');
        }
      })
      .then(data => {
        console.log('User:', data[0].userId);
        console.log('Roli:', data[0].userRole);
      })
      .catch(error => {
        console.error('Error:', error);
      });
  }, []); // Empty dependency array to run the effect only once

  return(
    <tr>
      <td>{ name }</td>
      <td>{ email }</td>
      <td>{ enrollnumber }</td>
      <td>
        <button onClick={ () => removeStudent(_id) } className="Action-Button fa fa-trash"></button>
        <Link to={{ pathname: '/edit', search: _id }}>
         <button className="Action-Button fa fa-pencil"></button>
        </Link>
      </td>

    </tr>
  );
};

export default Student;
