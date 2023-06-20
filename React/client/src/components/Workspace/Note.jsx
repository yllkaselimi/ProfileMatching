import React, { useState, useEffect } from 'react';
import axios from 'axios';

const Note = ({ workspaceId, userId }) => {
  const [noteText, setNoteText] = useState('');

  useEffect(() => {
    const fetchNote = async () => {
      try {
        const response = await axios.get(
          `http://localhost:5000/api/notes/forWorkspaceAndUser/${workspaceId}/${userId}`
        );
        const existingNote = response.data.notes[0];

        if (existingNote) {
          setNoteText(existingNote.noteText);
        }
      } catch (error) {
        console.error('Error:', error);
      }
    };

    fetchNote();
  }, [workspaceId, userId]);

  const handleSave = async () => {
    try {
      if (noteText) {
        const response = await axios.get(
          `http://localhost:5000/api/notes/forWorkspaceAndUser/${workspaceId}/${userId}`
        );
        const existingNote = response.data.notes[0];

        if (existingNote) {
          await axios.put(`http://localhost:5000/api/notes/${existingNote._id}`, {
            noteText: noteText,
          });
          console.log('Note updated');
        } else {
          await axios.post(`http://localhost:5000/api/notes`, {
            workspaceId: workspaceId,
            userId: userId,
            noteText: noteText,
          });
          console.log('Note created');
        }
      } else {
        console.log('Note text is empty');
      }
    } catch (error) {
      console.error('Error:', error);
    }
  };

  return (
    <div>
      <input
        type="text"
        value={noteText}
        onChange={(e) => setNoteText(e.target.value)}
      />
      <button onClick={handleSave}>Save</button>
    </div>
  );
};

export default Note;
