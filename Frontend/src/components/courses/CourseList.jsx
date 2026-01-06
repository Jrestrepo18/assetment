import React, { useState, useEffect } from 'react';
import { useSearchParams } from 'react-router-dom';
import { courseService } from '../../services/courseService';
import { lessonService } from '../../services/lessonService';
import CourseItem from './CourseItem';
import Modal from '../Modal';
import {
  Plus,
  Search,
  Filter,
  ChevronUp,
  ChevronDown,
  Edit3,
  Trash,
  Save,
  X,
  Zap,
  Rocket,
  Library,
  FileEdit,
  CheckCircle,
  AlertTriangle
} from 'lucide-react';

const CourseList = () => {
  const [courses, setCourses] = useState([]);
  const [totalPages, setTotalPages] = useState(1);
  const [totalCourses, setTotalCourses] = useState(0);
  const [draftCount, setDraftCount] = useState(0);
  const [publishedCount, setPublishedCount] = useState(0);
  const [loading, setLoading] = useState(true);
  const [searchParams, setSearchParams] = useSearchParams();

  // Modal states
  const [showCourseModal, setShowCourseModal] = useState(false);
  const [showEditModal, setShowEditModal] = useState(false);
  const [showLessonsModal, setShowLessonsModal] = useState(false);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [selectedCourse, setSelectedCourse] = useState(null);
  const [courseToDelete, setCourseToDelete] = useState(null);
  const [lessons, setLessons] = useState([]);
  const [newCourseTitle, setNewCourseTitle] = useState('');
  const [newCourseDescription, setNewCourseDescription] = useState('');
  const [editCourseTitle, setEditCourseTitle] = useState('');
  const [editCourseDescription, setEditCourseDescription] = useState('');
  const [newLessonTitle, setNewLessonTitle] = useState('');
  const [editingLessonId, setEditingLessonId] = useState(null);
  const [editLessonTitle, setEditLessonTitle] = useState('');

  const page = parseInt(searchParams.get('page')) || 1;
  const status = searchParams.get('status') || '';
  const q = searchParams.get('q') || '';

  const fetchCourses = async (showLoading = true) => {
    if (showLoading) setLoading(true);
    try {
      const params = { page, q };
      if (status) params.status = status;
      const data = await courseService.getAll(params);
      setCourses(data.items || data);
      setTotalPages(data.totalPages || 1);

      // Fetch global stats
      const stats = await courseService.getStats();
      setTotalCourses(stats.total);
      setDraftCount(stats.drafts);
      setPublishedCount(stats.published);
    } catch (error) {
      console.error("Error fetching courses", error);
    } finally {
      if (showLoading) setLoading(false);
    }
  };

  useEffect(() => {
    fetchCourses();
  }, [page, status, q]);

  const handleSearch = (e) => {
    if (e.key === 'Enter' || e.type === 'blur') {
      const newParams = new URLSearchParams(searchParams);
      if (e.target.value) newParams.set('q', e.target.value);
      else newParams.delete('q');
      newParams.set('page', '1');
      setSearchParams(newParams);
    }
  };

  const handleStatusFilter = (e) => {
    const newParams = new URLSearchParams(searchParams);
    if (e.target.value) newParams.set('status', e.target.value);
    else newParams.delete('status');
    newParams.set('page', '1');
    setSearchParams(newParams);
  };

  const initiateDelete = (course) => {
    setCourseToDelete(course);
    setShowDeleteModal(true);
  };

  const confirmDelete = async () => {
    if (!courseToDelete) return;
    try {
      await courseService.delete(courseToDelete.id);
      // Instant local update
      setCourses(courses.filter(c => c.id !== courseToDelete.id));
      setShowDeleteModal(false);
      setCourseToDelete(null);
      fetchCourses(false); // Refresh stats and list in background
    } catch (error) {
      console.error("Error deleting course", error);
    }
  };

  const handleCreateCourse = async (e) => {
    e.preventDefault();
    if (!newCourseTitle.trim()) return;

    try {
      await courseService.create({ title: newCourseTitle, description: newCourseDescription });
      setNewCourseTitle('');
      setNewCourseDescription('');
      setShowCourseModal(false);
      fetchCourses(false);
    } catch (error) {
      console.error("Error creating course", error);
    }
  };

  const openEditModal = (course) => {
    setSelectedCourse(course);
    setEditCourseTitle(course.title);
    setEditCourseDescription(course.description || '');
    setShowEditModal(true);
  };

  const handleEditCourse = async (e) => {
    e.preventDefault();
    if (!editCourseTitle.trim() || !selectedCourse) return;

    try {
      await courseService.update(selectedCourse.id, {
        title: editCourseTitle,
        description: editCourseDescription
      });
      setShowEditModal(false);
      fetchCourses(false);
    } catch (error) {
      console.error("Error updating course", error);
    }
  };

  const handlePublishToggle = async (course) => {
    try {
      if (course.status === 1 || course.status === 'Published') {
        await courseService.unpublish(course.id);
      } else {
        await courseService.publish(course.id);
      }
      fetchCourses(false);
    } catch (error) {
      alert(error.response?.data?.message || 'Error al cambiar estado');
    }
  };

  const openLessonsModal = async (course) => {
    setSelectedCourse(course);
    try {
      const lessonData = await lessonService.getByCourseId(course.id);
      setLessons(lessonData || []);
    } catch (error) {
      console.error("Error fetching lessons", error);
      setLessons([]);
    }
    setShowLessonsModal(true);
  };

  const handleAddLesson = async (e) => {
    e.preventDefault();
    if (!newLessonTitle.trim() || !selectedCourse) return;

    try {
      const nextOrder = lessons.length > 0 ? Math.max(...lessons.map(l => l.order)) + 1 : 1;
      await lessonService.create(selectedCourse.id, newLessonTitle, nextOrder);

      const lessonData = await lessonService.getByCourseId(selectedCourse.id);
      setLessons(lessonData || []);
      setNewLessonTitle('');
    } catch (error) {
      console.error("Error adding lesson", error);
      alert(error.response?.data?.message || 'Error al agregar lección');
    }
  };

  const handleDeleteLesson = async (lessonId) => {
    try {
      await lessonService.delete(lessonId);
      setLessons(lessons.filter(l => l.id !== lessonId));
    } catch (error) {
      console.error("Error deleting lesson", error);
    }
  };

  const startEditingLesson = (lesson) => {
    setEditingLessonId(lesson.id);
    setEditLessonTitle(lesson.title);
  };

  const handleUpdateLesson = async (id, currentOrder) => {
    if (!editLessonTitle.trim()) return;
    try {
      await lessonService.update(id, editLessonTitle, currentOrder);
      setEditingLessonId(null);
      const lessonData = await lessonService.getByCourseId(selectedCourse.id);
      setLessons(lessonData || []);
    } catch (error) {
      console.error("Error updating lesson", error);
    }
  };

  const handleReorderLesson = async (id, newOrder) => {
    try {
      await lessonService.reorder(id, newOrder);
      const lessonData = await lessonService.getByCourseId(selectedCourse.id);
      setLessons(lessonData || []);
    } catch (error) {
      console.error("Error reordering lesson", error);
    }
  };

  if (loading && courses.length === 0) return <div className="container"><p className="text-muted">Cargando cursos...</p></div>;

  return (
    <div className="container">
      {/* Counters */}
      <div className="stats-grid">
        <div className="stat-card stat-total">
          <div className="stat-icon-wrapper">
            <Library size={24} />
          </div>
          <div>
            <span className="stat-number">{totalCourses}</span>
            <span className="stat-label">Total Cursos</span>
          </div>
        </div>
        <div className="stat-card stat-draft">
          <div className="stat-icon-wrapper">
            <FileEdit size={24} />
          </div>
          <div>
            <span className="stat-number">{draftCount}</span>
            <span className="stat-label">Borradores</span>
          </div>
        </div>
        <div className="stat-card stat-published">
          <div className="stat-icon-wrapper">
            <CheckCircle size={24} />
          </div>
          <div>
            <span className="stat-number">{publishedCount}</span>
            <span className="stat-label">Publicados</span>
          </div>
        </div>
      </div>

      <div className="page-header">
        <div style={{ display: 'flex', alignItems: 'center', gap: '1rem' }}>
          <h1>Mis Cursos</h1>
          {loading && <span className="fetching-indicator">Cargando...</span>}
        </div>
        <button className="btn btn-primary" onClick={() => setShowCourseModal(true)}>
          <Plus size={18} /> Nuevo Curso
        </button>
      </div>

      {/* Filters */}
      <div className="filters-bar">
        <div className="search-input-wrapper">
          <Search size={16} className="search-icon" />
          <input
            type="text"
            placeholder="Buscar cursos..."
            className="form-control"
            defaultValue={q}
            onBlur={handleSearch}
            onKeyDown={handleSearch}
          />
        </div>
        <div className="filter-select-wrapper">
          <Filter size={16} className="filter-icon" />
          <select className="form-control" value={status} onChange={handleStatusFilter}>
            <option value="">Todos los estados</option>
            <option value="Draft">Borrador</option>
            <option value="Published">Publicado</option>
          </select>
        </div>
      </div>

      <div className="course-grid">
        {courses.length === 0 ? (
          <div className="empty-state">
            <Rocket size={48} className="empty-icon" />
            <p>No tienes cursos todavía.</p>
            <button className="btn btn-primary" onClick={() => setShowCourseModal(true)}>
              Crear tu primer curso
            </button>
          </div>
        ) : (
          courses.map(course => (
            <CourseItem
              key={course.id}
              course={course}
              onEdit={() => openEditModal(course)}
              onDelete={() => initiateDelete(course)}
              onManageLessons={() => openLessonsModal(course)}
              onPublishToggle={() => handlePublishToggle(course)}
            />
          ))
        )}
      </div>

      {totalPages > 1 && (
        <div className="pagination">
          {[...Array(totalPages)].map((_, i) => (
            <button
              key={i}
              className={`btn ${page === i + 1 ? 'btn-primary' : 'btn-outline'}`}
              onClick={() => {
                const newParams = new URLSearchParams(searchParams);
                newParams.set('page', (i + 1).toString());
                setSearchParams(newParams);
              }}
            >
              {i + 1}
            </button>
          ))}
        </div>
      )}

      {/* Modal Nuevo Curso */}
      <Modal isOpen={showCourseModal} onClose={() => setShowCourseModal(false)} title="Nuevo Curso">
        <form onSubmit={handleCreateCourse}>
          <div className="form-group">
            <label>Título del curso</label>
            <input
              type="text"
              className="form-control"
              value={newCourseTitle}
              onChange={e => setNewCourseTitle(e.target.value)}
              placeholder="Ej: Introducción a React"
              required
            />
          </div>
          <div className="form-group">
            <label>Descripción</label>
            <textarea
              className="form-control"
              value={newCourseDescription}
              onChange={e => setNewCourseDescription(e.target.value)}
              placeholder="Descripción del curso..."
              rows={3}
            />
          </div>
          <div className="modal-actions">
            <button type="button" className="btn btn-outline" onClick={() => setShowCourseModal(false)}>
              <X size={16} /> Cancelar
            </button>
            <button type="submit" className="btn btn-primary">
              <Plus size={16} /> Crear Curso
            </button>
          </div>
        </form>
      </Modal>

      {/* Modal Editar Curso */}
      <Modal isOpen={showEditModal} onClose={() => setShowEditModal(false)} title="Editar Curso">
        <form onSubmit={handleEditCourse}>
          <div className="form-group">
            <label>Título del curso</label>
            <input
              type="text"
              className="form-control"
              value={editCourseTitle}
              onChange={e => setEditCourseTitle(e.target.value)}
              required
            />
          </div>
          <div className="form-group">
            <label>Descripción</label>
            <textarea
              className="form-control"
              value={editCourseDescription}
              onChange={e => setEditCourseDescription(e.target.value)}
              placeholder="Descripción del curso..."
              rows={3}
            />
          </div>
          <div className="modal-actions">
            <button type="button" className="btn btn-outline" onClick={() => setShowEditModal(false)}>
              <X size={16} /> Cancelar
            </button>
            <button type="submit" className="btn btn-primary">
              <Save size={16} /> Guardar Cambios
            </button>
          </div>
        </form>
      </Modal>

      {/* Modal Lecciones */}
      <Modal
        isOpen={showLessonsModal}
        onClose={() => {
          setShowLessonsModal(false);
          setSelectedCourse(null);
          setEditingLessonId(null);
        }}
        title={selectedCourse ? `Lecciones: ${selectedCourse.title}` : 'Lecciones'}
      >
        <div className="lessons-list">
          {lessons.length === 0 ? (
            <p className="text-muted" style={{ textAlign: 'center', padding: '1rem' }}>
              No hay lecciones todavía.
            </p>
          ) : (
            lessons.map((lesson, index) => (
              <div key={lesson.id} className="lesson-item">
                <div className="lesson-left">
                  <div className="reorder-actions">
                    <button
                      className="btn-icon"
                      onClick={() => handleReorderLesson(lesson.id, lesson.order - 1)}
                      disabled={index === 0}
                    >
                      <ChevronUp size={14} />
                    </button>
                    <button
                      className="btn-icon"
                      onClick={() => handleReorderLesson(lesson.id, lesson.order + 1)}
                      disabled={index === lessons.length - 1}
                    >
                      <ChevronDown size={14} />
                    </button>
                  </div>
                  {editingLessonId === lesson.id ? (
                    <input
                      type="text"
                      className="form-control form-control-sm"
                      value={editLessonTitle}
                      onChange={(e) => setEditLessonTitle(e.target.value)}
                      onBlur={() => handleUpdateLesson(lesson.id, lesson.order)}
                      onKeyDown={(e) => {
                        if (e.key === 'Enter') handleUpdateLesson(lesson.id, lesson.order);
                        if (e.key === 'Escape') setEditingLessonId(null);
                      }}
                      autoFocus
                    />
                  ) : (
                    <span onClick={() => startEditingLesson(lesson)} className="lesson-title-text">
                      {index + 1}. {lesson.title}
                    </span>
                  )}
                </div>
                <div className="lesson-actions">
                  <button
                    className="btn btn-sm btn-outline"
                    onClick={() => startEditingLesson(lesson)}
                  >
                    <Edit3 size={12} />
                  </button>
                  <button
                    className="btn btn-sm btn-outline btn-danger"
                    onClick={() => handleDeleteLesson(lesson.id)}
                  >
                    <Trash size={12} />
                  </button>
                </div>
              </div>
            ))
          )}
        </div>
        <form onSubmit={handleAddLesson} className="add-lesson-form">
          <input
            type="text"
            className="form-control"
            value={newLessonTitle}
            onChange={e => setNewLessonTitle(e.target.value)}
            placeholder="Nueva lección..."
          />
          <button type="submit" className="btn btn-primary">
            <Zap size={14} /> Agregar
          </button>
        </form>
      </Modal>

      {/* Modal Confirmar Eliminación */}
      <Modal
        isOpen={showDeleteModal}
        onClose={() => setShowDeleteModal(false)}
        title="" // Empty title for cleaner look, icon takes focus
      >
        <div className="delete-confirmation">
          <div className="delete-warning-icon">
            <AlertTriangle size={42} strokeWidth={1.5} />
          </div>
          <h3>¿Estás seguro?</h3>
          <p>
            Estás a punto de eliminar el curso <strong>{courseToDelete?.title}</strong>.
            <br />Esta acción no se puede deshacer.
          </p>

          <div className="modal-actions delete-actions">
            <button
              type="button"
              className="btn btn-outline"
              onClick={() => setShowDeleteModal(false)}
            >
              Cancelar
            </button>
            <button
              type="button"
              className="btn btn-danger"
              onClick={confirmDelete}
            >
              <Trash size={16} /> Eliminar Definitivamente
            </button>
          </div>
        </div>
      </Modal>

    </div>
  );
};

export default CourseList;