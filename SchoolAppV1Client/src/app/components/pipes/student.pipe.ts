import { Pipe, PipeTransform } from '@angular/core';
import { StudentModel } from '../../Models/student.model';

@Pipe({
  name: 'student',
  standalone: true
})
export class StudentPipe implements PipeTransform {

  transform(value: StudentModel[], search:string):StudentModel[] {
    if(search == "") return value;
    return value.filter(p=> 
      p.fullName.toLowerCase().includes(search.toLowerCase()) || 
      p.identityNumber.includes(search) ||
      p.studentNumber.toString().includes(search)
  );
}
}
