import React, { useState } from 'react';
import axios from 'axios';

const AddCard = ({ match }) => {
  const [cardName, setCardName] = useState('');
  const [cardDescription, setCardDescription] = useState('');
  const [status, setStatus] = useState(false);
  const [deadline, setDeadline] = useState('');

  const boardId = match.params.boardId;
  const userId = match.params.userId;

  const handleCardNameChange = (event) => {
    setCardName(event.target.value);
  };

  const handleCardDescriptionChange = (event) => {
    setCardDescription(event.target.value);
  };

  const handleStatusChange = (event) => {
    setStatus(event.target.checked);
  };

  const handleDeadlineChange = (event) => {
    setDeadline(event.target.value);
  };

  const createCard = async (e) => {
    e.preventDefault();
    try {
      await axios.post('http://localhost:5000/api/cards', {
        boardId: boardId,
        cardName: cardName,
        cardDescription: cardDescription,
        userId: userId,
        status: status,
        deadline: deadline,
      });

      // Clear form inputs
      setCardName('');
      setCardDescription('');
      setStatus(false);
      setDeadline('');

      // Redirect to the workspace or any other appropriate page
      // Replace the below line with the desired redirection logic
      window.history.back();
    } catch (err) {
      console.log('Error:', err);
    }
  };

  return (
    <div>
      <h2>Add Card</h2>
      <form onSubmit={createCard}>
        <div>
          <label htmlFor="cardName">Card Name:</label>
          <input
            type="text"
            id="cardName"
            value={cardName}
            onChange={handleCardNameChange}
            required
          />
        </div>
        <div>
          <label htmlFor="cardDescription">Card Description:</label>
          <textarea
            id="cardDescription"
            value={cardDescription}
            onChange={handleCardDescriptionChange}
          />
        </div>
        <div>
          <label htmlFor="status">Status:</label>
          <input
            type="checkbox"
            id="status"
            checked={status}
            onChange={handleStatusChange}
          />
        </div>
        <div>
          <label htmlFor="deadline">Deadline:</label>
          <input
            type="date"
            id="deadline"
            value={deadline}
            onChange={handleDeadlineChange}
          />
        </div>
        <button type="submit">Create Card</button>
      </form>
    </div>
  );
};

export default AddCard;