
import {
  Component,
  OnInit,
  ChangeDetectorRef,
  inject
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { Pais, GuardarPais } from '../../core/models/pais.model';
import { PaisService } from '../../core/services/pais.service';

@Component({
  selector: 'app-paises',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './paises.html',
  styleUrl: './paises.css'
})
export class Paises implements OnInit {
  private readonly paisService = inject(PaisService);
  private readonly cdr = inject(ChangeDetectorRef);

  paises: Pais[] = [];

  formulario: GuardarPais = {
    nombre: '',
    codigoIso2: '',
    activo: true
  };

  paisEditando: number | null = null;
  cargando = false;
  guardando = false;
  mensaje = '';
  tipoMensaje: 'exito' | 'error' = 'exito';

  ngOnInit(): void {
    this.cargarPaises();
  }

  cargarPaises(): void {
    this.cargando = true;

    this.paisService.obtenerTodos().subscribe({
      next: (paises) => {
        this.paises = paises;
        this.cargando = false;

        this.cdr.markForCheck();
      },
      error: () => {
        this.mostrarMensaje(
          'No fue posible cargar los países. Verifica que la API esté ejecutándose.',
          'error'
        );

        this.cargando = false;
        this.cdr.markForCheck();
      }
    });
  }

  guardar(): void {
    const nombre = this.formulario.nombre.trim();
    const codigo = this.formulario.codigoIso2?.trim().toUpperCase() || null;
    const activo = this.formulario.activo;

    if (!nombre) {
      this.mostrarMensaje('El nombre del país es obligatorio.', 'error');
      return;
    }

    if (nombre.length > 100) {
      this.mostrarMensaje(
        'El nombre no puede superar los 100 caracteres.',
        'error'
      );
      return;
    }

    if (codigo && !/^[A-Z]{2}$/.test(codigo)) {
      this.mostrarMensaje(
        'El código ISO debe tener exactamente dos letras.',
        'error'
      );
      return;
    }

    const datos: GuardarPais = {
      nombre,
      codigoIso2: codigo,
      activo
    };

    this.guardando = true;
    this.mensaje = '';

    const peticion = this.paisEditando === null
      ? this.paisService.crear(datos)
      : this.paisService.actualizar(this.paisEditando, datos);

    peticion.subscribe({
      next: () => {
        this.mostrarMensaje(
          this.paisEditando === null
            ? 'País creado correctamente.'
            : 'País actualizado correctamente.',
          'exito'
        );

        this.limpiarFormulario();
        this.cargarPaises();
        this.guardando = false;
        this.cdr.markForCheck();
      },
      error: (error) => {
        const mensaje = error.error?.mensaje
          ?? error.error?.title
          ?? 'No fue posible guardar el país.';

        this.mostrarMensaje(mensaje, 'error');
        this.guardando = false;
        this.cdr.markForCheck();
      }
    });
  }

  editar(pais: Pais): void {
    this.paisEditando = pais.paisId;

    this.formulario = {
      nombre: pais.nombre,
      codigoIso2: pais.codigoIso2 ?? '',
      activo: pais.activo
    };

    this.mensaje = '';
  }

  eliminar(pais: Pais): void {
    const confirmar = window.confirm(
      `¿Deseas eliminar el país "${pais.nombre}"?`
    );

    if (!confirmar) {
      return;
    }

    this.mensaje = '';

    this.paisService.eliminar(pais.paisId).subscribe({
      next: () => {
        this.mostrarMensaje(
          'País eliminado correctamente.',
          'exito'
        );

        this.cargarPaises();
        this.cdr.markForCheck();
      },
      error: (error) => {
        const mensaje =
          error.error?.mensaje ??
          error.error?.detail ??
          error.error?.title ??
          'No fue posible eliminar el país. Puede tener departamentos asociados.';

        this.mostrarMensaje(mensaje, 'error');
        this.cdr.markForCheck();
      }
    });
  }


  limpiarFormulario(): void {
    this.paisEditando = null;

    this.formulario = {
      nombre: '',
      codigoIso2: '',
      activo: true
    };
  }

  private mostrarMensaje(
    mensaje: string,
    tipo: 'exito' | 'error'
  ): void {
    this.mensaje = mensaje;
    this.tipoMensaje = tipo;
  }
}
