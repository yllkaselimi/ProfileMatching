import React, { useState, useEffect } from 'react';
import axios from 'axios';
import AddCard from '../Card/AddCard';
import { Link } from 'react-router-dom';

const WorkspacePage = ({ match }) => {
  const [workspace, setWorkspace] = useState(null);
  const [showCreateBoard, setShowCreateBoard] = useState(false);
  const [boardName, setBoardName] = useState('');
  const [matchingBoards, setMatchingBoards] = useState([]);
  const [addingCard, setAddingCard] = useState(null);
  const [cardsByBoard, setCardsByBoard] = useState({});
  const workspaceId = match.params.id;
  const userId = match.params.userId;

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
    const fetchMatchingBoards = async () => {
      try {
        const response = await axios.get(`http://localhost:5000/api/boards/forWorkspace/${workspaceId}`);
        const matchingBoardsData = response.data;
        console.log('Matching Boards Data:', matchingBoardsData);
        setMatchingBoards(matchingBoardsData.boards);

        const cardsData = await Promise.all(
          matchingBoardsData.boards.map(async (board) => {
            const cardResponse = await axios.get(`http://localhost:5000/api/cards/getByBoard/${board._id}`);
            return { boardId: board._id, cards: cardResponse.data.cards };
          })
        );

        const cardsByBoardData = {};
        cardsData.forEach((cardsObj) => {
          cardsByBoardData[cardsObj.boardId] = cardsObj.cards;
        });
        setCardsByBoard(cardsByBoardData);
      } catch (error) {
        console.error('Error:', error);
      }
    };

    fetchMatchingBoards();
  }, [workspaceId]);

  const handleBoardNameChange = (event) => {
    setBoardName(event.target.value);
  };

  const toggleCreateBoard = () => {
    setShowCreateBoard(!showCreateBoard);
  };

  const createBoard = async (e) => {
    e.preventDefault();
    try {
      await axios.post('http://localhost:5000/api/boards', {
        workspaceId: workspaceId,
        boardName: boardName,
      });
      setBoardName('');

      const matchingResponse = await axios.get(`http://localhost:5000/api/boards/forWorkspace/${workspaceId}`);
      const matchingBoardsData = matchingResponse.data;
      setMatchingBoards(matchingBoardsData.boards);
    } catch (err) {
      console.log('Error:', err);
    }
  };

  const addCard = async (boardId) => {
    setAddingCard(boardId);
  };

  const handleCardAdded = async () => {
    const matchingResponse = await axios.get(`http://localhost:5000/api/boards/forWorkspace/${workspaceId}`);
    const matchingBoardsData = matchingResponse.data;
    setMatchingBoards(matchingBoardsData.boards);
    setAddingCard(null);
  };

  if (!workspace) {
    return <p>Loading workspace...</p>;
  }

  return (
    <div>
      <h2>Workspace Details</h2>
      <p>JobPost Name: {workspace.jobPostName}</p>

      <h3 style={{ display: 'flex', alignItems: 'center' }}>
        <button
          onClick={toggleCreateBoard}
          style={{ cursor: 'pointer', backgroundColor: 'transparent', border: 'none' }}
        >
          Add Board +
        </button>
        {showCreateBoard && (
          <form onSubmit={createBoard} style={{ display: 'flex', alignItems: 'center', marginLeft: '10px' }}>
            <input
              type="text"
              value={boardName}
              onChange={handleBoardNameChange}
              placeholder="Board Name"
              style={{ padding: '5px', marginRight: '5px', height: '24px' }}
            />
            <button type="submit" style={{ padding: '5px 10px', height: '24px' }}>
              Create
            </button>
          </form>
        )}
      </h3>

      <h3>Workspace Boards</h3>
      <div style={{ display: 'flex', flexWrap: 'wrap' }}>
        {matchingBoards.map((board) => (
          <div
            key={board.id}
            style={{
              backgroundColor: '#e9e9e9',
              padding: '20px',
              margin: '10px',
              width: '200px',
            }}
          >
            <p style={{ fontWeight: 'bold' }}>Board Name: {board.boardName}</p>
            <div>
              {cardsByBoard[board._id] &&
                cardsByBoard[board._id].map((card) => (
                  <div
                    key={card._id}
                    style={{
                      backgroundColor: '#ffffff',
                      border: '1px solid #e9e9e9',
                      borderRadius: '4px',
                      padding: '10px',
                      margin: '5px 0',
                    }}
                  >
                    <p style={{ fontWeight: 'bold', marginBottom: '5px' }}>Card Name:</p>
                    <p>{card.cardName}</p>
                    <p style={{ fontWeight: 'bold', marginBottom: '5px' }}>Card Description:</p>
                    <p>{card.cardDescription}</p>
                    <p style={{ fontWeight: 'bold', marginBottom: '5px' }}>Status:</p>
                    <p>{card.status ? 'Done' : 'Not Done'}</p>
                    <p style={{ fontWeight: 'bold', marginBottom: '5px' }}>Deadline:</p>
                    <p>{new Date(card.deadline).toLocaleDateString()}</p>
                  </div>
                ))}
            </div>
            {addingCard === board.id ? (
              <AddCard boardId={board._id} onCardAdded={handleCardAdded} />
            ) : (
              <Link to={`/addcard/${board._id}/${userId}`}>Add Card</Link>
            )}
          </div>
        ))}
      </div>
    </div>
  );
};

export default WorkspacePage;
