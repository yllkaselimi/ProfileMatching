import React, { useState, useEffect } from 'react';
import axios from 'axios';

const WorkspacePage = ({ match }) => {
    const [workspace, setWorkspace] = useState(null);
    const [boardName, setBoardName] = useState('');
    const [boards, setBoards] = useState([]); // State to store fetched boards
    const workspaceId = match.params.id;

    useEffect(() => {
        const fetchWorkspace = async () => {
            try {
                const response = await axios.get(`http://localhost:5000/api/workspaces/${workspaceId}`);
                const workspaceData = response.data;
                console.log('Workspace Data:', workspaceData);
                setWorkspace(workspaceData.workspace);
            } catch (error) {
                console.error('Error:', error);
            }
        };

        fetchWorkspace();
    }, [workspaceId]);

    useEffect(() => {
        const fetchBoards = async () => {
            try {
                const response = await axios.get(`http://localhost:5000/api/boards?workspaceId=${workspaceId}`);
                const boardsData = response.data;
                console.log('Boards Data:', boardsData);
                setBoards(boardsData.boards);
            } catch (error) {
                console.error('Error:', error);
            }
        };

        fetchBoards();
    }, [workspaceId]);

    const handleBoardNameChange = (event) => {
        setBoardName(event.target.value);
    };

    const createBoard = async (e) => {
        e.preventDefault();
        try {
            await axios.post('http://localhost:5000/api/boards', {
                workspaceId: workspaceId,
                boardName: boardName,
            });
            setBoardName('');

            // Refresh the boards data after creating the board
            const response = await axios.get(`http://localhost:5000/api/boards?workspaceId=${workspaceId}`);
            const boardsData = response.data;
            setBoards(boardsData.boards);
        } catch (err) {
            console.log('Error:', err);
        }
    };

    if (!workspace) {
        return <p>Loading workspace...</p>;
    }

    return (
        <div>
            <h2>Workspace Details</h2>
            <p>JobPost Name: {workspace.jobPostName}</p>
            {/* Display other relevant workspace details */}

            <h3>Create Board</h3>
            <form onSubmit={createBoard}>
                <label>
                    Board Name:
                    <input type="text" value={boardName} onChange={handleBoardNameChange} />
                </label>
                <button type="submit">Create</button>
            </form>

            <h3>Boards</h3>
            {boards.length > 0 ? (
                <div>
                    {boards.map((board) => (
                        <div key={board.id} className="board-box">
                            <p>Board Name: {board.name}</p>
                            {/* Display other relevant board details */}
                        </div>
                    ))}
                </div>
            ) : (
                <p>No boards found.</p>
            )}
        </div>
    );
};

export default WorkspacePage;