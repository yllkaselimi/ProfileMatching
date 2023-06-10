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
        console.log('Role:', data[0].userRole);

        // Check if the userRole is "Client"
        if (data[0].userRole === 'Client') {
          // Fetch data from the new URL for Client
          fetch('https://localhost:7044/api/ApplicantsPerJobs/GetJobpostsByClientId', { mode: 'cors', credentials: 'include' })
            .then(response => {
              if (response.ok) {
                return response.json(); // Parse the response as JSON
              } else {
                throw new Error('Error fetching job posts by client ID');
              }
            })
            .then(jobPostsData => {
              console.log('Job Posts (Client):', jobPostsData);
            })
            .catch(error => {
              console.error('Error:', error);
            });
        }

        // Check if the userRole is "Freelancer"
        if (data[0].userRole === 'Freelancer') {
          // Fetch data from the new URL for Freelancer
          fetch('https://localhost:7044/api/ApplicantsPerJobs/getmyhiredjobs', { mode: 'cors', credentials: 'include' })
            .then(response => {
              if (response.ok) {
                return response.json(); // Parse the response as JSON
              } else {
                throw new Error('Error fetching hired jobs');
              }
            })
            .then(hiredJobsData => {
              console.log('Hired Jobs (Freelancer):', hiredJobsData);
            })
            .catch(error => {
              console.error('Error:', error);
            });
        }
      })
      .catch(error => {
        console.error('Error:', error);
      });
  }, []);

  return null; // Render nothing since this component is only responsible for fetching user credentials
};

export default UserCredentials;
