import './Student.css';
import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import Avatar from 'react-avatar';


const Student = ({ _id, name, email, enrollnumber, removeStudent }) => {

  const [puna2, setPuna2] = useState(""); // Use state to store the value of puna2

  useEffect(() => {
    fetch('https://localhost:7044/api/jobpostsapi', { mode: 'cors', credentials: 'include' })
      .then(response => {
        return response.json(); // Parse the response as JSON
      })
      .then(data => {
        console.log('Name:', data); // Log the response data, which may contain the credentials
        setPuna2(data.jobPosts[2].jobPostName); // Update the value of puna2 using setState (setPuna2)
        console.log(puna2 + " qeee");
      })
      .catch(error => {
        console.error('Error:', error);
      });
  }, []); // Empty dependency array to run the effect only once

  return(
    <tr>
      <td>{ puna2 }</td>
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
