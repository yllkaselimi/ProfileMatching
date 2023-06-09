import React, { useEffect } from 'react';

const UserCredentials = () => {
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
  }, []);

  return null; // Render nothing since this component is only responsible for fetching user credentials
};

export default UserCredentials;