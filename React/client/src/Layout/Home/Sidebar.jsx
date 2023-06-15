import React, { useState, useEffect } from 'react';
import axios from 'axios';

const Sidebar = ({ userId, userRole }) => {
  const [jobPosts, setJobPosts] = useState([]);
  const [selectedJobPost, setSelectedJobPost] = useState('');
  const [workspaces, setWorkspaces] = useState([]);

  useEffect(() => {
    const fetchWorkspaces = async () => {
      try {
        const response = await axios.get('http://localhost:5000/api/workspaces');
        const workspacesData = response.data;
        setWorkspaces(workspacesData.workspaces);
      } catch (err) {
        console.log('Error:', err);
      }
    };

    fetchWorkspaces();
  }, []);

  useEffect(() => {
    if (userRole === 'Client') {
      fetch('https://localhost:7044/api/ApplicantsPerJobs/GetJobpostsByClientId', {
        mode: 'cors',
        credentials: 'include'
      })
        .then((response) => {
          if (response.ok) {
            return response.json();
          } else {
            throw new Error('Error fetching job posts by client ID');
          }
        })
        .then((jobPostsData) => {
          const matchingWorkspaces = workspaces.filter((workspace) =>
            jobPostsData.some((jobPost) => jobPost.jobPostId.toString() === workspace.jobPostId)
          );
          setWorkspaces(matchingWorkspaces);

          const unmatchedJobPosts = jobPostsData.filter(
            (jobPost) => !matchingWorkspaces.some((workspace) => workspace.jobPostId === jobPost.jobPostId.toString())
          );
          setJobPosts(unmatchedJobPosts);
        })
        .catch((error) => {
          console.error('Error:', error);
        });
    } else if (userRole === 'Freelancer') {
      fetch('https://localhost:7044/api/ApplicantsPerJobs/getmyhiredjobs', {
        mode: 'cors',
        credentials: 'include'
      })
        .then((response) => {
          if (response.ok) {
            return response.json();
          } else {
            throw new Error('Error fetching hired jobs by freelancer');
          }
        })
        .then((hiredJobsData) => {
          const matchingWorkspaces = workspaces.filter((workspace) =>
            hiredJobsData.some((jobPost) => jobPost.jobPostId.toString() === workspace.jobPostId)
          );
          setWorkspaces(matchingWorkspaces);
        })
        .catch((error) => {
          console.error('Error:', error);
        });
    }
  }, [userRole, workspaces]);

  const handleJobPostChange = (event) => {
    setSelectedJobPost(event.target.value);
  };

  const addWorkspace = async (e) => {
    e.preventDefault();
    try {
      const jobPostId = selectedJobPost;

      await axios.post('http://localhost:5000/api/workspaces/create-workspace', {
        jobPostId: jobPostId,
        userId: userId
      });

      setSelectedJobPost('');

      // Refresh the page to fetch updated workspaces
      window.location.reload();
    } catch (err) {
      console.log('Error:', err);
    }
  };

  return (
    <div className="sidebar bg-light p-3">
      <p>Welcome {userId} from userCredentials</p>
      {jobPosts.length > 0 && (
        <>
          <p>Add Workspace:</p>
          <div className="input-group mb-3">
            <select className="form-select" value={selectedJobPost} onChange={handleJobPostChange}>
              <option value="">Select Job Post</option>
              {jobPosts.map((jobPost) => (
                <option key={jobPost.jobPostId} value={jobPost.jobPostId}>
                  {jobPost.jobPostName}
                </option>
              ))}
            </select>
            <button className="btn btn-primary" onClick={addWorkspace} disabled={!selectedJobPost}>
              Add
            </button>
          </div>
        </>
      )}

      <p>Workspaces:</p>
      {workspaces.length > 0 ? (
        <ul>
          {workspaces.map((workspace) => (
            <li key={workspace.id}>{workspace.jobPostId}</li>
          ))}
        </ul>
      ) : (
        <p>Nothing to show here</p>
      )}
    </div>
  );
};

export default Sidebar;
