import React, { useEffect } from 'react';

const UserCredentials = ({ setUserCredentials }) => { // Accept setUserCredentials prop
  useEffect(() => {
    fetch('https://localhost:7044/api/jobpostsapi/getUserCredentials', { mode: 'cors', credentials: 'include' })
      .then((response) => {
        if (response.ok) {
          return response.json();
        } else if (response.status === 404) {
          window.location.href = 'https://localhost:7044/Identity/Account/Login';
        } else {
          throw new Error('Error fetching user credentials');
        }
      })
      .then((data) => {
        const userId = data[0].userId;
        const userRole = data[0].userRole;
        console.log('User:', userId);
        console.log('Role:', userRole);
        setUserCredentials(userId, userRole); // Call setUserCredentials with userId and userRole
      })
      .catch((error) => {
        console.error('Error:', error);
      });
  }, [setUserCredentials]);

  return null;
};

export default UserCredentials;
