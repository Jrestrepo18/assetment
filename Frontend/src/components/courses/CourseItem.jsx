import React from 'react';
import { useAuth } from '../../context/AuthContext';
import { BookOpen, Edit2, PlayCircle, StopCircle, Trash2 } from 'lucide-react';

// Helper para convertir status a string
const getStatusString = (status) => {
  if (typeof status === 'string') return status;
  return status === 1 ? 'Published' : 'Draft';
};

const CourseItem = ({ course, onEdit, onDelete, onManageLessons, onPublishToggle }) => {
  const { user } = useAuth();
  const statusStr = getStatusString(course.status);
  const isPublished = statusStr === 'Published';
  const isAdmin = user?.isAdmin;

  return (
    <div className="card">
      <div className="card-content">
        <div className="card-title-row">
          <h3>{course.title}</h3>
          <span className={`badge badge-${statusStr.toLowerCase()}`}>
            {isPublished ? 'Publicado' : 'Borrador'}
          </span>
        </div>
        {course.description && (
          <p className="text-muted">{course.description}</p>
        )}
      </div>
      <div className="card-footer">
        <button onClick={onManageLessons} className="btn btn-outline btn-sm" title="Gestionar Lecciones">
          <BookOpen size={18} />
        </button>
        <button onClick={onEdit} className="btn btn-outline btn-sm" title="Editar Curso">
          <Edit2 size={18} />
        </button>
        <button onClick={onPublishToggle} className="btn btn-outline btn-sm" title={isPublished ? 'Despublicar Curso' : 'Publicar Curso'}>
          {isPublished ? <StopCircle size={18} /> : <PlayCircle size={18} />}
        </button>
        {isAdmin && (
          <button onClick={onDelete} className="btn btn-outline btn-sm btn-danger" title="Eliminar Curso">
            <Trash2 size={18} />
          </button>
        )}
      </div>


    </div>
  );
};

export default CourseItem;

